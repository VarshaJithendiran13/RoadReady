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
    public class ReviewRepositoryTests
    {
        private Mock<YourDbContext> _mockDbContext;
        private ReviewRepository _reviewRepository;

        [SetUp]
        public void SetUp()
        {
            // Arrange: Setup mock DbContext and DbSet with asynchronous query support
            var mockReviewData = new List<Review>
            {
                new Review { ReviewId = 1, UserId = 1, CarId = 1, Rating = 5, Comment = "Excellent!", ReviewDate = System.DateTime.Now },
                new Review { ReviewId = 2, UserId = 2, CarId = 1, Rating = 4, Comment = "Good!", ReviewDate = System.DateTime.Now }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Review>>();
            mockDbSet.As<IQueryable<Review>>().Setup(m => m.Provider).Returns(mockReviewData.Provider);
            mockDbSet.As<IQueryable<Review>>().Setup(m => m.Expression).Returns(mockReviewData.Expression);
            mockDbSet.As<IQueryable<Review>>().Setup(m => m.ElementType).Returns(mockReviewData.ElementType);
            mockDbSet.As<IQueryable<Review>>().Setup(m => m.GetEnumerator()).Returns(mockReviewData.GetEnumerator());
            mockDbSet.As<IAsyncEnumerable<Review>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new AsyncEnumerator<Review>(mockReviewData.GetEnumerator()));

            _mockDbContext = new Mock<YourDbContext>();
            _mockDbContext.Setup(db => db.Reviews).Returns(mockDbSet.Object);

            // Setup SaveChangesAsync mock
            _mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _reviewRepository = new ReviewRepository(_mockDbContext.Object);
        }

        [Test]
        public async Task GetAllReviewsAsync_ReturnsAllReviews()
        {
            // Act
            var reviews = await _reviewRepository.GetAllReviewsAsync();

            // Assert
            Assert.That(reviews, Is.Not.Null);
            Assert.That(reviews.Count(), Is.EqualTo(2));
        }

      

      
        
        [Test]
        public void UpdateReviewAsync_ThrowsNotFoundException_WhenReviewDoesNotExist()
        {
            // Arrange
            var updatedReview = new Review { ReviewId = 999, Rating = 3, Comment = "Updated!" };

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _reviewRepository.UpdateReviewAsync(updatedReview));
            Assert.That(ex.Message, Is.EqualTo("Review with ID 999 not found."));
        }

        [Test]
        public void DeleteReviewAsync_ThrowsNotFoundException_WhenReviewDoesNotExist()
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _reviewRepository.DeleteReviewAsync(999));
            Assert.That(ex.Message, Is.EqualTo("Review with ID 999 not found."));
        }

       

      
    }
}
