using NUnit.Framework;
using Moq;
using CarRental.Models;
using CarRental.Repository;
using CarRental.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CarRental.Test.AsyncHelpers;

namespace CarRental.Tests
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private Mock<YourDbContext> _mockDbContext;
        private UserRepository _userRepository;

        [SetUp]
        public void SetUp()
        {
            // Arrange: Mock User data
            var mockUserData = new List<User>
            {
                new User { UserId = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", Password = "123456", PhoneNumber = "1234567890" },
                new User { UserId = 2, FirstName = "Jane", LastName = "Doe", Email = "jane@example.com", Password = "password", PhoneNumber = "0987654321" }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(mockUserData.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(mockUserData.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(mockUserData.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(mockUserData.GetEnumerator());
            mockDbSet.As<IAsyncEnumerable<User>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new AsyncEnumerator<User>(mockUserData.GetEnumerator()));

            _mockDbContext = new Mock<YourDbContext>();
            _mockDbContext.Setup(db => db.Users).Returns(mockDbSet.Object);

            // Mock SaveChangesAsync
            _mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _userRepository = new UserRepository(_mockDbContext.Object);
        }

        [Test]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Act
            var users = await _userRepository.GetAllUsersAsync();

            // Assert
            Assert.That(users, Is.Not.Null);
            Assert.That(users.Count(), Is.EqualTo(2));
        }

        
       
        
        

        [Test]
        public void UpdateUserPasswordAsync_ThrowsNotFoundException_WhenUserDoesNotExist()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _userRepository.UpdateUserPasswordAsync(999, "newPassword"));
            Assert.That(ex.Message, Is.EqualTo("User with ID 999 not found."));
        }

        

      


    }
}
