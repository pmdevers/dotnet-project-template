using Template.Api.Configuration;
using Template.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddServiceDefaults()
    .AddLoggerConfigs();

using var loggerFactory = LoggerFactory.Create(config => config.AddConsole());
var startupLogger = loggerFactory.CreateLogger<Program>();

builder
    .AddOptionConfigs(startupLogger)
    .AddServiceConfigs(startupLogger)
    .AddInfrastructure(startupLogger);

var app = builder.Build();

app
    .MapDefaultEndpoints()
    .UseAppMiddleware();

await app.RunAsync();
