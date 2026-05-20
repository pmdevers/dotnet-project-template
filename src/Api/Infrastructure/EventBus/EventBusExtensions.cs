using Template.Api.Domain.Abstractions;

namespace Template.Api.Infrastructure.EventBus;

public static class EventBusExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddEventBus()
            => services.AddWhenNotRegisterd<IEventBus>(s => {
                s.AddSingleton<MessageQueue>();
                s.AddSingleton<IEventBus, EventBus>();
                s.AddSingleton<IHostedService, EventBusBackgroundService>();
            });
    }
}
