using Microsoft.EntityFrameworkCore;

namespace Template.Api.Infrastructure.Data;

public class AppDbContextMigrationService(IServiceScopeFactory scopeFactory, ILogger<AppDbContextMigrationService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        logger.LogInformation("Applying pending database migrations...");
        await db.Database.MigrateAsync(cancellationToken);
        logger.LogInformation("Database migrations applied successfully.");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
