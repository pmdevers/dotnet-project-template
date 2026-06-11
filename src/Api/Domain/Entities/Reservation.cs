using Template.Api.Domain.Abstractions;
using Template.Api.Domain.ValueObjects;

namespace Template.Api.Domain.Entities;


[GenerateId]
public class Reservation : AggregateRoot
{
    public ReservationId Id { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public LicensePlate LicensePlate { get; private set; }

    public ReservationDate StartDate { get; private set; }
    public ReservationDate EndDate { get; private set; }

    public ReservationStatus Status { get; private set; } = ReservationStatus.InProgress;

    public static Reservation Create(CustomerId customerId, ReservationDate startDate, ReservationDate endDate)
    {
        var reservation = new Reservation();
        reservation.RecordEvent(new ReservationCreatedEvent(ReservationId.New(), customerId, startDate, endDate));
        return reservation;
    }

    public void AttachCar(LicensePlate licensePlate)
    {
        RecordEvent(new ReservationCarAttachedEvent(Id, licensePlate));
    }

    internal void Apply(ReservationCreatedEvent @event)
    {
        Id = @event.ReservationId;
        CustomerId = @event.CustomerId;
        StartDate = @event.StartDate;
        EndDate = @event.EndDate;
    }

    internal void Apply(ReservationCarAttachedEvent @event)
    {
        LicensePlate = @event.LicensePlate;
    }
}

public enum ReservationStatus
{
    InProgress,
    Accepted,
    Completed,
    Cancelled
}

public record ReservationCreatedEvent(ReservationId ReservationId, CustomerId CustomerId, ReservationDate StartDate, ReservationDate EndDate) : DomainEvent;
public record ReservationCarAttachedEvent(ReservationId ReservationId, LicensePlate LicensePlate) : DomainEvent;
