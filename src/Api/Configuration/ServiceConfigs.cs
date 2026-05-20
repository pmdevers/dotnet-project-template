using Template.Api.Infrastructure;

namespace Template.Api.Configuration;

public static class ServiceConfigs
{
    extension(IServiceCollection services)
    {

        public IServiceCollection AddServiceConfigs(ILogger logger, WebApplicationBuilder builder)
        {
            services.AddInfrastructure(builder.Configuration, logger);

            services.AddOpenApi();

            logger.LogInformation("{Project} services registered", "Services");

            return services;
        }
    }
}
