using Template.Api.Domain.Abstractions;
using Template.Api.Domain.ValueObjects;

namespace Template.Api.Domain.Entities;

public class Car : AggregateRoot
{
    public LicensePlate LicensePlate { get; private set; }

    public static Car Create(LicensePlate licensePlate)
    {
        var car = new Car();

        car.RecordEvent(new CarCreatedEvent(licensePlate));

        return car;
    }

    internal void Apply(CarCreatedEvent @event)
    {
        LicensePlate = @event.LicensePlate;
    }
}

public record CarCreatedEvent(LicensePlate LicensePlate) : DomainEvent;
