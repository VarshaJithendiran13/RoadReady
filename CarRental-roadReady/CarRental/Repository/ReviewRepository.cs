using CarRental.Models;
using CarRental.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly YourDbContext _context;

        public ReviewRepository(YourDbContext context)
        {
            _context = context;
        }

        // Get all reviews
        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            try
            {
                return await _context.Reviews.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception and throw a custom InternalServerException
                throw new InternalServerException($"An error occurred while retrieving the reviews: {ex.Message}");
            }
        }

        // Get a review by ID
        public async Task<Review> GetReviewByIdAsync(int reviewId)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);

            if (review == null)
                throw new NotFoundException($"Review with ID {reviewId} not found.");

            return review;
        }

        // Add a new review
        public async Task AddReviewAsync(Review review)
        {
            if (review == null)
                throw new ValidationException("Review details cannot be null.");

            // Check for duplicate review if needed
            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == review.UserId && r.CarId == review.CarId);

            if (existingReview != null)
                throw new DuplicateResourceException("A review already exists for this car by this user.");

            try
            {
                await _context.Reviews.AddAsync(review);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Catch database-related issues and rethrow as InternalServerException
                throw new InternalServerException($"An error occurred while adding the review: {ex.Message}");
            }
        }

        // Update an existing review
        public async Task UpdateReviewAsync(Review updatedReview)
        {
            if (updatedReview == null)
                throw new ValidationException("Updated review details cannot be null.");

            var existingReview = await _context.Reviews.FindAsync(updatedReview.ReviewId);

            if (existingReview == null)
                throw new NotFoundException($"Review with ID {updatedReview.ReviewId} not found.");

            // Update review details
            existingReview.Rating = updatedReview.Rating;
            existingReview.Comment = updatedReview.Comment;
            existingReview.ReviewDate = updatedReview.ReviewDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InternalServerException($"An error occurred while updating the review: {ex.Message}");
            }
        }

        // Delete a review by ID
        public async Task DeleteReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);

            if (review == null)
                throw new NotFoundException($"Review with ID {reviewId} not found.");

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
        // Get reviews by car ID
        public async Task<IEnumerable<Review>> GetReviewsByCarIdAsync(int carId)
        {
            try
            {
                var reviews = await _context.Reviews
                    .Where(r => r.CarId == carId)
                    .ToListAsync();

                if (reviews == null || reviews.Count == 0)
                    throw new NotFoundException($"No reviews found for car ID {carId}.");

                return reviews;
            }
            catch (Exception ex)
            {
                // Log the exception and throw a custom InternalServerException
                throw new InternalServerException($"An error occurred while retrieving the review for car {carId}: {ex.Message}");
            }
        }

        
    }
}
