using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Template.Api.Configuration;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Infrastructure.EventBus;

public sealed class RedisEventBus(IConnectionMultiplexer redis, IOptions<EventBusOptions> options) : IEventBus
{
    private readonly IDatabase _db = redis.GetDatabase();
    private readonly EventBusOptions _options = options.Value;

    public async ValueTask PublishAsync<T>(T message, CancellationToken ct = default)
        where T : DomainEvent
    {
        var payload = EventSerializer.Serialize(message);

        await _db.StreamAddAsync(
            _options.StreamName,
            [
                new NameValueEntry("type", message.GetType().FullName),
                new NameValueEntry("payload", payload)
            ]).ConfigureAwait(false);
    }
}
