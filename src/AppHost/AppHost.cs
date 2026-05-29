using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

#pragma warning disable ASPIRECOMPUTE003 // AddContainerRegistry is an experimental API and may change in future releases.
var registery = builder.AddContainerRegistry("ghcr", "ghcr.io", "pmdevers");
#pragma warning restore ASPIRECOMPUTE003 // AddContainerRegistry is an experimental API and may change in future releases.

var imageName = Environment.GetEnvironmentVariable("IMAGE_NAME") ?? "my-template-app";
var imageTag = Environment.GetEnvironmentVariable("IMAGE_TAG") ?? "latest";
var version = Environment.GetEnvironmentVariable("VERSION") ?? "0.0.1";

var kubernetes = builder.AddKubernetesEnvironment("homelab")
    .WithHelm(helm =>
    {
        helm.WithChartName("my-template-app")
            .WithChartVersion(imageTag)
            .WithChartDescription("My template application deployed to Kubernetes")
            .WithReleaseName("template-app")
            .WithNamespace("template");
});

var cache = builder.AddRedis("cache");

var postgres = builder.AddPostgres("appdb")
    .WithPgAdmin()
    .WithLifetime(ContainerLifetime.Persistent);

#pragma warning disable ASPIRECOMPUTE003 // WithContainerRegistry is an experimental API and may change in future releases.
#pragma warning disable ASPIREPIPELINES003 // WithImagePushOptions is an experimental API and may change in future releases.
builder.AddProject<Projects.Template_Api>("template-api")
    .WithContainerRegistry(registery)
    .WithImagePushOptions((context) =>
    {
        context.Options.RemoteImageTag = imageTag;
    })
    .WithHttpHealthCheck("/health")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", builder.Environment.EnvironmentName)
    .WithReference(cache)
    .WithReference(postgres)
    .WithUrl("http://localhost:5000");
#pragma warning restore ASPIREPIPELINES003 // WithImagePushOptions is an experimental API and may change in future releases.
#pragma warning restore ASPIRECOMPUTE003 // WithContainerRegistry is an experimental API and may change in future releases.


builder.Build().Run();
