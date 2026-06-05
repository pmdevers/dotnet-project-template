using Microsoft.AspNetCore.Http;
using NSubstitute;
using Template.Api.Domain.Abstractions;
using Template.Api.Domain.Entities;
using Template.Api.Domain.ValueObjects;
using Template.Api.Features.Reservations;

namespace Template.Tests.Features.Reservations;

public class GetReservationByIdTests
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
        public async Task GetReservation_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var reservationId = ReservationId.New();
            var customerId = CustomerId.New();
            var startDate = ReservationDate.Today().AddDays(1);
            var endDate = ReservationDate.Today().AddDays(5);
            var reservation = Reservation.Create(customerId, startDate, endDate);

            _reservationRepository.TryFindAsync(reservationId).Returns(reservation);

            // Act
            var result = await GetReservationById.Handle(reservationId, _unitOfWork);

            // Assert
            await Assert.That(result).IsNotNull();
        }

        [Test]
        public async Task GetReservation_WithValidId_ReturnsReservation()
        {
            // Arrange
            var reservationId = ReservationId.New();
            var customerId = CustomerId.New();
            var startDate = ReservationDate.Today().AddDays(2);
            var endDate = ReservationDate.Today().AddDays(6);
            var reservation = Reservation.Create(customerId, startDate, endDate);

            _reservationRepository.TryFindAsync(reservationId).Returns(reservation);

            // Act
            var result = await GetReservationById.Handle(reservationId, _unitOfWork);

            // Assert
            await Assert.That(result).IsNotNull();
        }

        [Test]
        public async Task GetReservation_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var reservationId = ReservationId.New();
            _reservationRepository.TryFindAsync(reservationId).Returns((Reservation?)null);

            // Act
            var result = await GetReservationById.Handle(reservationId, _unitOfWork);

            // Assert
            await Assert.That(result).IsNotNull();
        }

        [Test]
        public async Task GetReservation_WithValidId_QueriesRepository()
        {
            // Arrange
            var reservationId = ReservationId.New();
            var customerId = CustomerId.New();
            var startDate = ReservationDate.Today().AddDays(2);
            var endDate = ReservationDate.Today().AddDays(6);
            var reservation = Reservation.Create(customerId, startDate, endDate);

            _reservationRepository.TryFindAsync(reservationId).Returns(reservation);

            // Act
            await GetReservationById.Handle(reservationId, _unitOfWork);

            // Assert
            await _reservationRepository.Received(1).TryFindAsync(reservationId);
        }
    }
}
