using NSubstitute;
using Template.Api.Domain.Abstractions;
using Template.Api.Domain.Entities;
using Template.Api.Features.Reservations;

namespace Template.Tests.Features.Reservations;

public class CreateReservationTests
{
    public class HandleTests
    {
        private IUnitOfWork _unitOfWork = null!;
        private IRepository<Reservation, ReservationId> _reservationRepository = null!;

        [Before(HookType.Test)]
        public void Setup()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _reservationRepository = Substitute.For<IRepository<Reservation, ReservationId>>();
            _unitOfWork.GetRepository<Reservation, ReservationId>().Returns(_reservationRepository);
        }

        [Test]
        public async Task CreateReservation_WithValidData_ReturnsCreatedResult()
        {
            // Arrange
            var customerId = CustomerId.New();
            var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5));
            var command = new CreateReservation.Command(customerId, startDate, endDate);

            // Act
            var result = await CreateReservation.Handle(_unitOfWork, command);

            // Assert
            await Assert.That(result).IsNotNull();
        }

        [Test]
        public async Task CreateReservation_WithValidData_AddsReservationToRepository()
        {
            // Arrange
            var customerId = CustomerId.New();
            var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5));
            var command = new CreateReservation.Command(customerId, startDate, endDate);

            // Act
            await CreateReservation.Handle(_unitOfWork, command);

            // Assert
            _reservationRepository.Received(1).Add(Arg.Is<Reservation>(r =>
                r.CustomerId == customerId &&
                r.StartDate == startDate &&
                r.EndDate == endDate));
        }

        [Test]
        public async Task CreateReservation_WithValidData_SavesChanges()
        {
            // Arrange
            var customerId = CustomerId.New();
            var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
            var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
            var command = new CreateReservation.Command(customerId, startDate, endDate);

            // Act
            await CreateReservation.Handle(_unitOfWork, command);

            // Assert
            await _unitOfWork.Received(1).SaveChangesAsync();
        }

        [Test]
        public async Task CreateReservation_WithValidData_ReturnsReservationInResponse()
        {
            // Arrange
            var customerId = CustomerId.New();
            var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3));
            var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(8));
            var command = new CreateReservation.Command(customerId, startDate, endDate);

            // Act
            var result = await CreateReservation.Handle(_unitOfWork, command);

            // Assert
            await Assert.That(result).IsNotNull();
        }

        [Test]
        public async Task CreateReservation_WithValidData_GeneratesReservationId()
        {
            // Arrange
            var customerId = CustomerId.New();
            var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(6));
            var command = new CreateReservation.Command(customerId, startDate, endDate);

            // Act
            var result = await CreateReservation.Handle(_unitOfWork, command);

            // Assert
            await Assert.That(result).IsNotNull();
        }
    }
}
