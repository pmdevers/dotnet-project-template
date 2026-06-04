using Template.Api.Domain.Abstractions;

namespace Template.Api.Infrastructure.EventBus;

public class EventBusBackgroundService(MessageQueue queue, IServiceScopeFactory scopeFactory, ILogger<EventBusBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var message in queue.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var handlerType = typeof(DomainEventHandler<>).MakeGenericType(message.GetType());
                var handlers = scope.ServiceProvider.GetServices(handlerType);

                foreach (var h in handlers)
                {
                    if (h is Delegate del)
                    {
                        var task = (Task)del.DynamicInvoke(message, stoppingToken)!;
                        await task;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error handling message {Message}", message.MessageId);
            }
        }
    }
}
