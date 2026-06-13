using Template.Api.Domain.Abstractions;
using Template.Api.Domain.ValueObjects;

namespace Template.Api.Domain.Entities;

public record Specifications(uint Seats, uint Luggage, Transmission Transmission, FuelType FuelType, uint HorsePower);

public class Car : AggregateRoot
{
    public LicensePlate LicensePlate { get; private set; }
    public NonEmptyString Name { get; private set; }
    public NonEmptyString Description { get; private set; }
    public NonEmptyString Brand { get; private set; }
    public NonEmptyString Model { get; private set; }
    public Category Category { get; private set; }
    public Money PricePerDay { get; private set; }
    public string[] Images { get; private set; } = [];
    public Specifications? Specifications { get; private set; }

    public static Car Create(LicensePlate licensePlate,
        NonEmptyString name,
        NonEmptyString description,
        NonEmptyString brand,
        NonEmptyString model,
        Category category,
        Money pricePerDay)
    {
        var car = new Car();

        car.RecordEvent(new CarCreatedEvent(licensePlate, name, description, brand, model, category, pricePerDay));

        return car;
    }

    public void SetSpecifications(uint seats, uint luggage, Transmission transmission, FuelType fuelType, uint horsePower)
    {
        RecordEvent(new CarSpecificationsSetEvent(LicensePlate, seats, luggage, transmission, fuelType, horsePower));
    }

    internal void Apply(CarCreatedEvent @event)
    {
        LicensePlate = @event.LicensePlate;
        Name = @event.Name;
        Description = @event.Description;
        Brand = @event.Brand;
        Model = @event.Model;
        Category = @event.Category;
        PricePerDay = @event.PricePerDay;
    }

    internal void Apply(CarSpecificationsSetEvent @event)
    {
        Specifications = new Specifications(@event.Seats, @event.Luggage, @event.Transmission, @event.FuelType, @event.HorsePower);
    }
}

public record CarCreatedEvent(LicensePlate LicensePlate, NonEmptyString Name, NonEmptyString Description, NonEmptyString Brand, NonEmptyString Model, Category Category, Money PricePerDay) : DomainEvent;
public record CarSpecificationsSetEvent(LicensePlate LicensePlate, uint Seats, uint Luggage, Transmission Transmission, FuelType FuelType, uint HorsePower) : DomainEvent;
