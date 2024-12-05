using NUnit.Framework;
using Moq;
using CarRental.Models;
using CarRental.Repository;
using CarRental.DTOs;
using CarRental.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using System.Threading;
using CarRental.Test;
using CarRental.Test.AsyncHelpers;

namespace CarRental.Tests
{
    [TestFixture]
    public class CarRepositoryTests
    {
        private Mock<YourDbContext> _mockDbContext;
        private CarRepository _carRepository;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            // Arrange: Setup AutoMapper configuration
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Car, CarReadDTO>();
                cfg.CreateMap<CarCreateDTO, Car>();
                cfg.CreateMap<CarUpdateDTO, Car>();
            });
            _mapper = config.CreateMapper();

            // Arrange: Setup mock DbContext and DbSet with asynchronous query support
            var mockCarData = new List<Car>
            {
                new Car { CarId = 1, Make = "Skoda", Model = "Kylaq", Specifications = "Convertible", Year = 2021, PricePerDay = 1200.00M, AvailabilityStatus = true, Location = "Mumbai", ImageUrl = "swift.jpg" },
                new Car { CarId = 2, Make = "MG", Model = "Hector", Specifications = "Convertible", Year = 2022, PricePerDay = 1400.00M, AvailabilityStatus = true, Location = "Bangalore", ImageUrl = "i20.jpg" },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Car>>();
            mockDbSet.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(mockCarData.Provider);
            mockDbSet.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(mockCarData.Expression);
            mockDbSet.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(mockCarData.ElementType);
            mockDbSet.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(mockCarData.GetEnumerator());
            mockDbSet.As<IAsyncEnumerable<Car>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new AsyncEnumerator<Car>(mockCarData.GetEnumerator()));

            _mockDbContext = new Mock<YourDbContext>();
            _mockDbContext.Setup(db => db.Cars).Returns(mockDbSet.Object);

            // Setup SaveChangesAsync mock
            _mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1); // Simulate a successful save.

            _carRepository = new CarRepository(_mockDbContext.Object);
        }

        [Test]
        public async Task GetAllCarsAsync_ReturnsAllCars()
        {
            // Act: Call the method
            var cars = await _carRepository.GetAllCarsAsync();

            // Assert: Check if the method returns all cars
            Assert.That(cars, Is.Not.Null);
            Assert.That(cars.Count(), Is.EqualTo(2));
        }

        [Test]
        public void AddCarAsync_ThrowsValidationException_WhenCarIsNull()
        {
            // Act & Assert: Check if ValidationException is thrown when car is null
            var ex = Assert.ThrowsAsync<ValidationException>(async () => await _carRepository.AddCarAsync(null));

            Assert.That(ex.Message, Is.EqualTo("Car details cannot be null."));
        }

        [Test]
        public void UpdateCarAsync_ThrowsNotFoundException_WhenCarNotFound()
        {
            // Arrange: Try updating a non-existing car
            var carUpdateDto = new CarUpdateDTO { CarId = 999, Make = "Maruti", Model = "Swift Updated", Year = 2021, PricePerDay = 1300.00M, AvailabilityStatus = true, Location = "Mumbai", ImageUrl = "swift.jpg" };
            var car = _mapper.Map<Car>(carUpdateDto);

            // Act & Assert: Check if NotFoundException is thrown
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _carRepository.UpdateCarAsync(car));

            Assert.That(ex.Message, Is.EqualTo("Car with ID 999 not found."));
        }

        [Test]
        public void DeleteCarAsync_ThrowsNotFoundException_WhenCarNotFound()
        {
            // Act & Assert: Try deleting a car that doesn't exist
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _carRepository.DeleteCarAsync(999));

            Assert.That(ex.Message, Is.EqualTo("Car with ID 999 not found."));
        }
    }
}
