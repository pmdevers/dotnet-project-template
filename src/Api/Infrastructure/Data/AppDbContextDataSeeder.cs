using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Template.Api.Configuration;
using Template.Api.Domain.Abstractions;
using Template.Api.Domain.Entities;
using Template.Api.Domain.ValueObjects;

namespace Template.Api.Infrastructure.Data;

public class AppDbContextDataSeeder(AppDbContext dbContext, IOptions<DatabaseOptions> options, ILogger<AppDbContextDataSeeder> logger) : IDataSeeder
{
    public DatabaseOptions? Options { get; } = options?.Value;

    public async Task SeedAsync(bool recreate, CancellationToken cancellationToken)
    {
        try
        {
            if(recreate && Options?.RecreateOnStartup == true)
            {
                logger.LogWarning("DROPPING database for fresh start (DatabaseOptions:RecreateOnStartup = true)...");
                await dbContext.Database.EnsureDeletedAsync(cancellationToken);
                logger.LogInformation("Database dropped.");
            }

            logger.LogInformation("Applying pending database migrations...");
            await dbContext.Database.MigrateAsync(cancellationToken);
            logger.LogInformation("Database migrations applied successfully.");

            await SeedInitialDataAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }

    private async Task SeedInitialDataAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var carRepo = dbContext.GetRepository<Car, CarId>();
        var reservationRepo = dbContext.GetRepository<Reservation, ReservationId>();

        var car = Car.Create(new LicensePlate("GPP-30-T"));
        var reservation = Reservation.Create(CustomerId.New(), DateOnly.Parse("2024-07-01"), DateOnly.Parse("2024-07-10"));

        carRepo.Add(car);
        reservationRepo.Add(reservation);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
