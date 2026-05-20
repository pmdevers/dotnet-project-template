using System.Threading.Channels;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Infrastructure.EventBus;

public class MessageQueue
{
    private readonly Channel<DomainEvent> _channel = Channel.CreateUnbounded<DomainEvent>();
    public ChannelReader<DomainEvent> Reader => _channel.Reader;
    public ChannelWriter<DomainEvent> Writer => _channel.Writer;
}
