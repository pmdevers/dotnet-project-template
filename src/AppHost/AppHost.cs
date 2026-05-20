var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");
var postgres = builder.AddPostgres("appdb")
    .WithLifetime(ContainerLifetime.Persistent);

builder.AddProject<Projects.Template_Api>("api")
    .WithHttpHealthCheck("/health")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
    .WithReference(cache)
    .WithReference(postgres)
    .WithUrl("http://localhost:5000");


builder.Build().Run();
