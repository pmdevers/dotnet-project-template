using Scalar.AspNetCore;
using Template.Api.Domain.Abstractions;
using Template.Api.Features;
using Template.WebUi;


namespace Template.Api.Configuration;

public static class MiddlewareConfig
{
    extension(WebApplication app)
    {
        public async Task<WebApplication> UseAppMiddleware()
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            var fileProvider = WebApp.CreateFileProvider();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = fileProvider,
                RequestPath = ""
            });

            app.MapApiEndpoints();

            app.MapFallback(async context =>
            {
                var file = fileProvider.GetFileInfo("index.html");
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync(file);
            });

            await app.SeedDatabase();

            return app;
        }

        private async Task SeedDatabase()
        {
            using var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
            await seeder.SeedAsync(app.Environment.IsDevelopment());
        }
    }
}
