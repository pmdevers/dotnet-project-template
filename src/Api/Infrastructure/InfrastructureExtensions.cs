using Template.Api.Infrastructure.Data;
using Template.Api.Infrastructure.EventBus;

namespace Template.Api.Infrastructure;

public static class InfrastructureExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration, ILogger logger)
        {
            var connectionString = configuration.GetConnectionString("appdb")
                ?? throw new InvalidOperationException("DefaultConnection is not set in the configuration.");

            services.AddAppDbContext(connectionString);
            services.AddEventBus();
            return services;
        }

        public IServiceCollection AddWhenNotRegisterd<T>(Action<IServiceCollection> register)
        {
            if (services.Any(x => x.ServiceType == typeof(T)))
            {
                return services;
            }

            register(services);
            return services;
        }
    }


}
