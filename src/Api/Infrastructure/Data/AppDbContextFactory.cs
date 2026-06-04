using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Template.Api.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(
                Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")
                ?? throw new InvalidOperationException("POSTGRES_CONNECTION_STRING environment variable is not set."))
            .Options;

        return new AppDbContext(options);
    }
}

