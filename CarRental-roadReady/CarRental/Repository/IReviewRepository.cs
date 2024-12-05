using CarRental.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Repository
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review> GetReviewByIdAsync(int reviewId);
        Task<IEnumerable<Review>> GetReviewsByCarIdAsync(int carId);


        Task AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(int reviewId);
    }
}
