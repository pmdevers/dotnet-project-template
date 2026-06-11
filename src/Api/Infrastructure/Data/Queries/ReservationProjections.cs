using Microsoft.EntityFrameworkCore;
using Template.Api.Domain.Entities;
using Template.Api.Domain.ValueObjects;
using Template.Api.Infrastructure.Data.Models;

namespace Template.Api.Infrastructure.Data.Queries;

public class ReservationProjections(AppDbContext dbContext)
{
    public async Task CreateReservation(ReservationCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        dbContext.Reservations.Add(new ReservationModel
        {
            Id = @event.ReservationId,
            LicensePlate = LicensePlate.Create("UNKNOWN"),
        });

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteReservation(ReservationCarAttachedEvent @event, CancellationToken cancellationToken = default)
    {
        var reservation = await dbContext.Reservations.FirstAsync(x => x.Id == @event.ReservationId, cancellationToken);

        reservation.LicensePlate = @event.LicensePlate;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
