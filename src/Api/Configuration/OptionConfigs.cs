using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using Template.Api.Domain.Abstractions;
using Microsoft.Extensions.Logging;

namespace Template.Api.Configuration;

public static class OptionConfigs
{
    extension(WebApplicationBuilder builder)
    {
        public WebApplicationBuilder AddOptionConfigs(ILogger logger)
        {
            var services = builder.Services;
            var configuration = builder.Configuration;
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

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("{Project} were configured", "Options");
            }

            return builder;
        }
    }


}
