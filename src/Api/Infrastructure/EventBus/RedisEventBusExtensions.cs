using Template.Api.Domain.Abstractions;

namespace Template.Api.Infrastructure.EventBus;

public static class RedisEventBusExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddRedisEventBus()
        {
            services.AddSingleton<IEventBus, RedisEventBus>();
            services.AddHostedService<RedisEventConsumer>();

            return services;
        }
    }
}
