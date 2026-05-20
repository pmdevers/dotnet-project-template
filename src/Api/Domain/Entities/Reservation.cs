using Template.Api.Domain.Abstractions;

namespace Template.Api.Domain.Entities;

[GenerateId]
public class Reservation : AggregateRoot
{
    public ReservationId Id { get; private set; }
    public CustomerId CustomerId { get; private set;  }
    public CarId CarId { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }

    public ReservationStatus Status { get; private set; } = ReservationStatus.InProgress;

    public static Reservation Create(CustomerId customerId, DateOnly startDate, DateOnly endDate)
    {
        var reservation = new Reservation();
        reservation.RecordEvent(new ReservationCreatedEvent(ReservationId.New(), customerId, startDate, endDate));
        return reservation;
    }

    public void AttachCar(CarId carId)
    {
        if(CarId == CarId.Empty)
        {
           throw new ArgumentNullException(nameof(CarId));
        }
        RecordEvent(new ReservationCarAttachedEvent(Id, carId));
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
        CarId  = @event.CarId;
    }
}

public enum ReservationStatus
{
    InProgress,
    Accepted,
    Completed,
    Cancelled
}

public record ReservationCreatedEvent(ReservationId ReservationId, CustomerId CustomerId, DateOnly StartDate, DateOnly EndDate) : DomainEvent;
public record ReservationCarAttachedEvent(ReservationId ReservationId, CarId CarId) : DomainEvent;
