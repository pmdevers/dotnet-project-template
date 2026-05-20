using Microsoft.AspNetCore.Mvc;
using Template.Api.Domain.Abstractions;
using Template.Api.Domain.Entities;

namespace Template.Api.Features.Reservations;

public static class CreateReservation
{
    public record Command(CarId CarId, DateOnly StartDate, DateOnly EndDate);

    public static async Task<IResult> Handle(
        [FromServices] IUnitOfWork uow,
        [FromBody]Command command
    )
    {
        var reservationRepo = uow.GetRepository<Reservation, ReservationId>();

        var reservation = Reservation.Create();

        reservationRepo.Add(reservation);

        await uow.SaveChangesAsync();

        return Results.Created($"/reservations/{reservation.Id}", reservation);
    }
}
