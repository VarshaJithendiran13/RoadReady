using NUnit.Framework;
using Moq;
using CarRental.Models;
using CarRental.Repository;
using CarRental.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using CarRental.Test.AsyncHelpers;

namespace CarRental.Tests
{
    [TestFixture]
    public class ReservationRepositoryTests
    {
        private Mock<YourDbContext> _mockDbContext;
        private ReservationRepository _reservationRepository;

        [SetUp]
        public void SetUp()
        {
            var mockReservationData = new List<Reservation>
            {
                new Reservation { ReservationId = 1, UserId = 1, CarId = 1, PickupDate = new DateTime(2023, 12, 1), DropoffDate = new DateTime(2023, 12, 5), TotalPrice = 4800.00M, ReservationStatus = "Confirmed" },
                new Reservation { ReservationId = 2, UserId = 2, CarId = 2, PickupDate = new DateTime(2023, 12, 10), DropoffDate = new DateTime(2023, 12, 15), TotalPrice = 7000.00M, ReservationStatus = "Pending" },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Reservation>>();
            mockDbSet.As<IQueryable<Reservation>>().Setup(m => m.Provider).Returns(mockReservationData.Provider);
            mockDbSet.As<IQueryable<Reservation>>().Setup(m => m.Expression).Returns(mockReservationData.Expression);
            mockDbSet.As<IQueryable<Reservation>>().Setup(m => m.ElementType).Returns(mockReservationData.ElementType);
            mockDbSet.As<IQueryable<Reservation>>().Setup(m => m.GetEnumerator()).Returns(mockReservationData.GetEnumerator());
            mockDbSet.As<IAsyncEnumerable<Reservation>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new AsyncEnumerator<Reservation>(mockReservationData.GetEnumerator()));

            _mockDbContext = new Mock<YourDbContext>();
            _mockDbContext.Setup(db => db.Reservations).Returns(mockDbSet.Object);
            _mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _reservationRepository = new ReservationRepository(_mockDbContext.Object);
        }

        [Test]
        public async Task GetAllReservationsAsync_ReturnsAllReservations()
        {
            var reservations = await _reservationRepository.GetAllReservationsAsync();

            Assert.That(reservations, Is.Not.Null);
            Assert.That(reservations.Count(), Is.EqualTo(2));
        }



        [Test]
        public void AddReservationAsync_ThrowsValidationException_WhenReservationIsNull()
        {
            var ex = Assert.ThrowsAsync<ValidationException>(async () => await _reservationRepository.AddReservationAsync(null));

            Assert.That(ex.Message, Is.EqualTo("Reservation details cannot be null."));
        }



        [Test]
        public void UpdateReservationAsync_ThrowsNotFoundException_WhenReservationNotFound()
        {
            var updatedReservation = new Reservation { ReservationId = 999, CarId = 1, PickupDate = DateTime.Now, DropoffDate = DateTime.Now.AddDays(5), TotalPrice = 5000.00M, ReservationStatus = "Confirmed" };

            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _reservationRepository.UpdateReservationAsync(updatedReservation));

            Assert.That(ex.Message, Is.EqualTo("Reservation with ID 999 not found."));
        }

        [Test]
        public void DeleteReservationAsync_ThrowsNotFoundException_WhenReservationNotFound()
        {
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _reservationRepository.DeleteReservationAsync(999));

            Assert.That(ex.Message, Is.EqualTo("Reservation with ID 999 not found."));
        }

    }
}
