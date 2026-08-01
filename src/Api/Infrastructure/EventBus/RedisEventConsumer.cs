using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Template.Api.Configuration;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Infrastructure.EventBus;

public sealed class RedisEventConsumer(
    IConnectionMultiplexer redis,
    IServiceProvider serviceProvider,
    IOptions<EventBusOptions> options,
    ILogger<RedisEventConsumer> logger
) : BackgroundService
{
    private const int BatchSize = 10;
    private static readonly TimeSpan IdleDelay = TimeSpan.FromSeconds(1);

    private readonly IDatabase _db = redis.GetDatabase();
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly EventBusOptions _options = options.Value;
    private readonly ILogger<RedisEventConsumer> _logger = logger;
    private bool _pendingDrainAttempted;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await EnsureGroupExistsAsync();

        while (!stoppingToken.IsCancellationRequested)
        {
            StreamEntry[] entries = await GetMessages();

            if (entries.Length == 0)
            {
                await Task.Delay(IdleDelay, stoppingToken);
                continue;
            }

            foreach (var entry in entries)
            {
                try
                {
                    await HandleEntryAsync(entry, stoppingToken);
                    await _db.StreamAcknowledgeAsync(_options.StreamName, _options.ConsumerGroup, entry.Id);
                }
                catch (Exception ex)
                {
                    _logger.RedisStreamEntryHandlingError(ex, entry.Id.ToString());
                }
            }
        }
    }

    private async Task<StreamEntry[]> GetMessages()
    {
        StreamEntry[] entries = [];

        if (_options.ReadFromBeginning && !_pendingDrainAttempted)
        {
            entries = await _db.StreamReadGroupAsync(
                _options.StreamName,
                _options.ConsumerGroup,
                _options.ConsumerName,
                position: StreamPosition.Beginning,
                count: BatchSize);

            if (entries.Length == 0)
            {
                _pendingDrainAttempted = true;
            }
        }

        if (entries.Length == 0)
        {
            entries = await _db.StreamReadGroupAsync(
                _options.StreamName,
                _options.ConsumerGroup,
                _options.ConsumerName,
                position: StreamPosition.NewMessages,
                count: BatchSize);
        }

        return entries;
    }

    private async Task EnsureGroupExistsAsync()
    {
        try
        {
            await _db.StreamCreateConsumerGroupAsync(
                _options.StreamName,
                _options.ConsumerGroup,
                position: _options.ReadFromBeginning ? StreamPosition.Beginning : StreamPosition.NewMessages,
                createStream: true);
        }
        catch (RedisServerException ex) when (ex.Message.Contains("BUSYGROUP"))
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.RedisConsumerGroupAlreadyExists(ex, _options.ConsumerGroup, _options.StreamName);
            }
        }
    }

    private async Task HandleEntryAsync(StreamEntry entry, CancellationToken ct)
    {
        var typeName = entry.Values.First(x => x.Name == "type").Value.ToString();
        var payload = entry.Values.First(x => x.Name == "payload").Value.ToString();

        var domainEvent = EventSerializer.Deserialize(payload, typeName);

        if (domainEvent is null)
            return;

        var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();

        var handlerType = typeof(DomainEventHandler<>).MakeGenericType(domainEvent.GetType());
        var handlers = scope.ServiceProvider.GetServices(handlerType);

        foreach (var h in handlers)
        {
            if (h is Delegate del)
            {
                var task = (Task)del.DynamicInvoke(domainEvent, ct)!;
                await task;
            }
        }
    }
}
