using Microsoft.AspNetCore.Mvc;
using Template.Api.Domain.Abstractions;
using Template.Api.Domain.Entities;
using Template.Api.Domain.ValueObjects;

namespace Template.Api.Features.Reservations;

public static class CreateReservation
{
    public record Command(CustomerId CustomerId, ReservationDate StartDate, ReservationDate EndDate);

    public static async Task<IResult> Handle(
        [FromServices] IUnitOfWork uow,
        [FromBody]Command command
    )
    {
        var reservationRepo = uow.GetRepository<Reservation, ReservationId>();

        var reservation = Reservation.Create(command.CustomerId, command.StartDate, command.EndDate);

        reservationRepo.Add(reservation);

        await uow.SaveChangesAsync();

        return TypedResults.Created($"/reservations/{reservation.Id}", reservation);
    }
}
