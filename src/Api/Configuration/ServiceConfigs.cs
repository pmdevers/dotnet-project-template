namespace Template.Api.Configuration;

public static class ServiceConfigs
{
    extension(IServiceCollection services)
    {

        public IServiceCollection AddServiceConfigs(ILogger logger, WebApplicationBuilder builder)
        {
            services.AddOpenApi();

            logger.LogInformation("{Project} services registered", "Services");

            return services;
        }
    }
}
