using Template.Api.Domain.Abstractions;
using Template.Api.Domain.ValueObjects;

namespace Template.Api.Domain.Entities;

[GenerateId]
public class Car : AggregateRoot
{
    public CarId Id { get; private set; }
    public LicensePlate LicensePlate { get; private set; } = default!;

    public static Car Create(LicensePlate licensePlate)
    {
        var car = new Car();

        car.RecordEvent(new CarCreatedEvent(CarId.New(), licensePlate));

        return car;
    }

    internal void Apply(CarCreatedEvent @event)
    {
        Id = @event.CarId;
        LicensePlate = @event.LicensePlate;
    }
}

public record CarCreatedEvent(CarId CarId, LicensePlate LicensePlate) : DomainEvent;
