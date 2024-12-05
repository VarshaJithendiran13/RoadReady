using Moq;
using CarRental.Models;
using CarRental.Repository;
using CarRental.Exceptions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using System.Threading;
using CarRental.Models.DTOs;
using CarRental.Test.AsyncHelpers;

namespace CarRental.Tests
{
    [TestFixture]
    public class PaymentRepositoryTests
    {
        private Mock<YourDbContext> _mockDbContext;
        private PaymentRepository _paymentRepository;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            // Arrange: Setup AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Payment, PaymentDTO>();
                cfg.CreateMap<CreatePaymentDTO, Payment>();
               
            });
            _mapper = config.CreateMapper();

            // Arrange: Setup mock DbContext and DbSet with asynchronous query support
            var mockPaymentData = new List<Payment>
            {
                new Payment { PaymentId = 1, Amount = 100, ReservationId = 1, PaymentDate = System.DateTime.Now, PaymentMethod = "Credit Card", Status = "Paid" },
                new Payment { PaymentId = 2, Amount = 200, ReservationId = 2, PaymentDate = System.DateTime.Now, PaymentMethod = "PayPal", Status = "Pending" },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Payment>>();
            mockDbSet.As<IQueryable<Payment>>().Setup(m => m.Provider).Returns(mockPaymentData.Provider);
            mockDbSet.As<IQueryable<Payment>>().Setup(m => m.Expression).Returns(mockPaymentData.Expression);
            mockDbSet.As<IQueryable<Payment>>().Setup(m => m.ElementType).Returns(mockPaymentData.ElementType);
            mockDbSet.As<IQueryable<Payment>>().Setup(m => m.GetEnumerator()).Returns(mockPaymentData.GetEnumerator());
            mockDbSet.As<IAsyncEnumerable<Payment>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new AsyncEnumerator<Payment>(mockPaymentData.GetEnumerator()));

            _mockDbContext = new Mock<YourDbContext>();
            _mockDbContext.Setup(db => db.Payments).Returns(mockDbSet.Object);

            // Setup SaveChangesAsync mock
            _mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1); // Simulate a successful save.

            _paymentRepository = new PaymentRepository(_mockDbContext.Object);
        }

        [Test]
        public async Task GetAllPaymentsAsync_ReturnsAllPayments()
        {
            // Act: Call the method
            var payments = await _paymentRepository.GetAllPaymentsAsync();

            // Assert: Check if the method returns all payments
            Assert.That(payments, Is.Not.Null);
            Assert.That(payments.Count(), Is.EqualTo(2));
        }

       


        [Test]
        public void AddPaymentAsync_ThrowsValidationException_WhenPaymentIsNull()
        {
            // Act & Assert: Check if ValidationException is thrown when payment is null
            var ex = Assert.ThrowsAsync<ValidationException>(async () => await _paymentRepository.AddPaymentAsync(null));

            Assert.That(ex.Message, Is.EqualTo("Payment details cannot be null."));
        }


       

       

        [Test]
        public async Task DeletePaymentAsync_ThrowsNotFoundException_WhenPaymentNotFound()
        {
            // Act & Assert: Check if NotFoundException is thrown when payment is not found
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _paymentRepository.DeletePaymentAsync(999));

            Assert.That(ex.Message, Is.EqualTo("Payment with ID 999 not found."));
        }


      

      
    }
}
