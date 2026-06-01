using Template.Api.Configuration;
using Template.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddLoggerConfigs();

using var loggerFactory = LoggerFactory.Create(config => config.AddConsole());
var startupLogger = loggerFactory.CreateLogger<Program>();

builder.Services
    .AddOptionConfigs(builder.Configuration, startupLogger, builder)
    .AddServiceConfigs(startupLogger, builder)
    .AddInfrastructure(builder.Configuration, startupLogger);

var app = builder.Build();

app.MapDefaultEndpoints();

await app.UseAppMiddleware();

await app.RunAsync();
