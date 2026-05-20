using Template.Api.Domain.Abstractions;
using Template.Api.Domain.Entities;

namespace Template.Api.Features.Reservations;

public class GetReservationById
{
    public static async Task<IResult> Handle(ReservationId id, IUnitOfWork uow)
    {
        var reservationRepo = uow.GetRepository<Reservation, ReservationId>();
        
        var reservation = await reservationRepo.TryFindAsync(id);
        
        if (reservation == null)
            return TypedResults.NotFound();
        
        return TypedResults.Ok(reservation);
    }
}
