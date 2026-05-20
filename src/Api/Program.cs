using Template.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddLoggerConfigs();

using var loggerFactory = LoggerFactory.Create(config => config.AddConsole());
var startupLogger = loggerFactory.CreateLogger<Program>();


builder.Services.AddOptionConfigs(builder.Configuration, startupLogger, builder);
builder.Services.AddServiceConfigs(startupLogger, builder);

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseAppMiddleware();

app.Run();
