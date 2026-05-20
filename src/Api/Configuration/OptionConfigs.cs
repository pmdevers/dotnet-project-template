using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;

namespace Template.Api.Configuration;

public static class OptionConfigs
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddOptionConfigs(IConfiguration configuration,
                                                      ILogger logger,
                                                      WebApplicationBuilder builder)
        {
            services
            //.Configure<MailserverConfiguration>(configuration.GetSection("Mailserver"))
            .Configure<DatabaseOptions>(configuration.GetSection("DatabaseOptions"))
            
            // Configure Web Behavior
            .Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            })
            .Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            if (builder.Environment.IsDevelopment())
            {
                
            }

            logger.LogInformation("{Project} were configured", "Options");

            return services;
        }
    }


}
