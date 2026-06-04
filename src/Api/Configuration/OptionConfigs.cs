using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using Template.Api.Domain.Abstractions;

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
                options.SerializerOptions.Converters.Add(new ValueObjectJsonConverter());
            });

            logger.LogInformation("{Project} were configured", "Options");

            return services;
        }
    }


}
