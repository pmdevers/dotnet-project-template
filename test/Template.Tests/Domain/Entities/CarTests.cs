using Template.Api.Domain.Entities;
using Template.Api.Domain.ValueObjects;

namespace Template.Tests.Domain.Entities;

public class CarTests
{
    [Test]
    public async Task RegisterCar_CreatesCarWithValidLicensePlate()
    {
        // Arrange
        var licensePlate = new LicensePlate("ABC123");

        // Act
        var car = Car.Create(licensePlate);

        // Assert
        await Assert.That(car.Id).IsNotEqualTo(CarId.Empty);
        await Assert.That(car.LicensePlate).IsEqualTo(licensePlate);
        await Assert.That(car.LicensePlate.ToJsonValue()).IsEqualTo("ABC123");
    }

    [Test]
    public async Task RegisterCar_GeneratesUniqueIds()
    {
        // Arrange
        var licensePlate1 = new LicensePlate("ABC123");
        var licensePlate2 = new LicensePlate("XYZ789");

        // Act
        var car1 = Car.Create(licensePlate1);
        var car2 = Car.Create(licensePlate2);

        // Assert
        await Assert.That(car1.Id).IsNotEqualTo(car2.Id);
    }

    [Test]
    public async Task RegisterCar_NormalizesLicensePlate()
    {
        // Arrange
        var licensePlate = new LicensePlate("abc123");

        // Act
        var car = Car.Create(licensePlate);

        // Assert
        await Assert.That(car.LicensePlate.ToJsonValue()).IsEqualTo("ABC123");
    }

    [Test]
    public async Task RegisterCar_RecordsCreatedEvent()
    {
        // Arrange
        var licensePlate = new LicensePlate("ABC123");

        // Act
        var car = Car.Create(licensePlate);
        var events = car.GetUncommittedEvents();

        // Assert
        await Assert.That(events.Count()).IsGreaterThanOrEqualTo(1);
        var createdEvent = events.First() as CarCreatedEvent;
        await Assert.That(createdEvent).IsNotNull();
        await Assert.That(createdEvent!.LicensePlate).IsEqualTo(licensePlate);
    }
}
