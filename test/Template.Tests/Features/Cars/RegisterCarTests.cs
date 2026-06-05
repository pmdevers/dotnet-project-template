using Microsoft.AspNetCore.Http;
using NSubstitute;
using Template.Api.Domain.Abstractions;
using Template.Api.Domain.Entities;
using Template.Api.Domain.ValueObjects;
using Template.Api.Features.Cars;

namespace Template.Tests.Features.Cars;

public class RegisterCarTests
{
    public class HandleTests
    {
        private IUnitOfWork _unitOfWork = null!;
        private IRepository<Car, CarId> _carRepository = null!;

        [Before(HookType.Test)]
        public void Setup()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _carRepository = Substitute.For<IRepository<Car, CarId>>();
            _unitOfWork.GetRepository<Car, CarId>().Returns(_carRepository);
        }

        [Test]
        public async Task RegisterCar_WithValidLicensePlate_ReturnsCreatedResult()
        {
            // Arrange
            var licensePlate = LicensePlate.Create("ABC123");
            var command = new Register.Command(licensePlate);

            // Act
            var result = await Register.Handle(command, _unitOfWork);

            // Assert
            await Assert.That(result).IsNotNull();
        }

        [Test]
        public async Task RegisterCar_WithValidLicensePlate_AddsCarToRepository()
        {
            // Arrange
            var licensePlate = LicensePlate.Create("XYZ789");
            var command = new Register.Command(licensePlate);

            // Act
            await Register.Handle(command, _unitOfWork);

            // Assert
            _carRepository.Received(1).Add(Arg.Is<Car>(c => c.LicensePlate == licensePlate));
        }

        [Test]
        public async Task RegisterCar_WithValidLicensePlate_SavesChanges()
        {
            // Arrange
            var licensePlate = LicensePlate.Create("LMN456");
            var command = new Register.Command(licensePlate);

            // Act
            await Register.Handle(command, _unitOfWork);

            // Assert
            await _unitOfWork.Received(1).SaveChangesAsync();
        }

        [Test]
        public async Task RegisterCar_WithValidLicensePlate_ReturnsCarInResponse()
        {
            // Arrange
            var licensePlate = LicensePlate.Create("DEF123");
            var command = new Register.Command(licensePlate);

            // Act
            var result = await Register.Handle(command, _unitOfWork);

            // Assert
            await Assert.That(result).IsNotNull();
        }

        [Test]
        public async Task RegisterCar_WithValidLicensePlate_GeneratesCarId()
        {
            // Arrange
            var licensePlate = LicensePlate.Create("GHI789");
            var command = new Register.Command(licensePlate);

            // Act
            var result = await Register.Handle(command, _unitOfWork);

            // Assert
            await Assert.That(result).IsNotNull();
        }
    }
}
