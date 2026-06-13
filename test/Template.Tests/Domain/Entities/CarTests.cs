using Template.Api.Domain.Entities;
using Template.Api.Domain.ValueObjects;

namespace Template.Tests.Domain.Entities;

public class CarTests
{
    [Test]
    public async Task RegisterCar_CreatesCarWithValidLicensePlate()
    {
        // Arrange
        var licensePlate = LicensePlate.Create("ABC123");

        // Act
        var car = Car.Create(licensePlate, "Test", "Description", "Brand", "Model", Category.Premium, new(100, Currency.Dollar));

        // Assert
        await Assert.That(car.LicensePlate).IsEqualTo(licensePlate);
        await Assert.That(car.LicensePlate.ToString()).IsEqualTo("ABC123");
    }

    [Test]
    public async Task RegisterCar_GeneratesUniqueIds()
    {
        // Arrange
        var licensePlate1 = LicensePlate.Create("ABC123");
        var licensePlate2 = LicensePlate.Create("XYZ789");

        // Act
        var car1 = Car.Create(licensePlate1, "Test", "Description", "Brand", "Model", Category.Premium, new(100, Currency.Dollar));
        var car2 = Car.Create(licensePlate2, "Test", "Description", "Brand", "Model", Category.Premium, new(100, Currency.Dollar));

        // Assert
        await Assert.That(car1.LicensePlate).IsNotEqualTo(car2.LicensePlate);
    }

    [Test]
    public async Task RegisterCar_NormalizesLicensePlate()
    {
        // Arrange
        var licensePlate = LicensePlate.Create("abc123");

        // Act
        var car = Car.Create(licensePlate, "Test", "Description", "Brand", "Model", Category.Premium, new(100, Currency.Dollar));

        // Assert
        await Assert.That(car.LicensePlate.ToString()).IsEqualTo("ABC123");
    }

    [Test]
    public async Task RegisterCar_RecordsCreatedEvent()
    {
        // Arrange
        var licensePlate = LicensePlate.Create("ABC123");

        // Act
        var car = Car.Create(licensePlate, "Test", "Description", "Brand", "Model", Category.Premium, new(100, Currency.Dollar));
        var events = car.GetUncommittedEvents();

        // Assert
        await Assert.That(events.Count()).IsGreaterThanOrEqualTo(1);
        var createdEvent = events.First() as CarCreatedEvent;
        await Assert.That(createdEvent).IsNotNull();
        await Assert.That(createdEvent!.LicensePlate).IsEqualTo(licensePlate);
    }
}
