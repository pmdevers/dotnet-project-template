using Microsoft.Extensions.FileProviders;

namespace Template.WebUi;

public static class WebApp
{
    public static IFileProvider CreateFileProvider()
    {
        var assembly = typeof(WebApp).Assembly;
        return new ManifestEmbeddedFileProvider(assembly, "dist");
    }
}
