using Template.Api.Infrastructure.Data;
using Template.Api.Infrastructure.EventBus;

namespace Template.Api.Infrastructure;

public static class InfrastructureExtensions
{
    extension(WebApplicationBuilder builder)
    {
        public WebApplicationBuilder AddInfrastructure(ILogger logger)
        {
            var configuration = builder.Configuration;
            var services = builder.Services;

            var connectionString = configuration.GetConnectionString("appdb")
                ?? throw new InvalidOperationException("DefaultConnection is not set in the configuration.");


            builder.AddRedisClient("cache");

            services.AddAppDbContext(connectionString);

            services.AddRedisEventBus();

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Infrastructure Services Registered.");
            }

            return builder;
        }
    }
}
