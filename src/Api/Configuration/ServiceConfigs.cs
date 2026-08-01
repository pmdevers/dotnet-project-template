namespace Template.Api.Configuration;

public static class ServiceConfigs
{
    extension(WebApplicationBuilder builder)
    {

        public WebApplicationBuilder AddServiceConfigs(ILogger logger)
        {
            var services = builder.Services;

            services.AddOptions();
            services.AddOpenApi();

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.ServicesRegistered("Configuration");
            }

            return builder;
        }
    }
}
