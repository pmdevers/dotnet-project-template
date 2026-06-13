using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Template.Api.Configuration;
using Template.Api.Domain.Entities;
using Template.Api.Domain.ValueObjects;

namespace Template.Api.Infrastructure.Data;

public class AppDbContextMigrationService(
    IServiceScopeFactory scopeFactory,
    IOptions<DatabaseOptions> options,
    ILogger<AppDbContextMigrationService> logger)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (options.Value?.RecreateOnStartup == true)
            {
                logger.LogWarning("DROPPING database for fresh start (DatabaseOptions:RecreateOnStartup = true)...");
                await dbContext.Database.EnsureDeletedAsync(cancellationToken);
                logger.LogInformation("Database dropped.");
            }

            logger.LogInformation("Applying pending database migrations...");
            await dbContext.Database.MigrateAsync(cancellationToken);
            logger.LogInformation("Database migrations applied successfully.");

            if (options.Value?.RecreateOnStartup == true)
            {
                await SeedInitialDataAsync(dbContext, cancellationToken);
            }

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static async Task SeedInitialDataAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var carRepo = dbContext.GetRepository<Car, LicensePlate>();
        var reservationRepo = dbContext.GetRepository<Reservation, ReservationId>();

        var car = Car.Create(LicensePlate.Create("GPP-30-T"), "Car Name", "Car Description", "Car Brand", "Car Model", Category.Standard, new Money(100m, Currency.Dollar));

        car.SetSpecifications(4, 2, Transmission.Automatic, FuelType.Petrol, 150);

        var reservation = Reservation.Create(CustomerId.New(), ReservationDate.Today(), ReservationDate.Today().AddDays(10));

        reservation.AttachCar(car.LicensePlate);

        carRepo.Add(car);
        reservationRepo.Add(reservation);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
