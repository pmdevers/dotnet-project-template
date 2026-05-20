using Template.Api.Domain.Abstractions;

namespace Template.Api.Domain.Entities;

[GenerateId]
public class Reservation : AggregateRoot
{
    public ReservationId Id { get; private set; }

    public static Reservation Create()
    {
        var reservation = new Reservation();
        reservation.RecordEvent(new ReservationCreatedEvent(ReservationId.New()));
        return reservation;
    }

    internal void Apply(ReservationCreatedEvent @event)
    {
        Id = @event.ReservationId;
    }
}

public record ReservationCreatedEvent(ReservationId ReservationId) : DomainEvent;
