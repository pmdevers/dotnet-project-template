using Template.Api.Domain.Abstractions;

namespace Template.Api.Infrastructure.EventBus;

public class EventBus(MessageQueue queue) : IEventBus
{
    public ValueTask PublishAsync<T>(T message, CancellationToken ct = default)
        where T : DomainEvent
    {
        return queue.Writer.WriteAsync(message, ct);
    }
}
