using DekatMe.Api.Models;

namespace DekatMe.Api.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<IEnumerable<Review>> GetReviewsByBusinessIdAsync(string businessId);
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(string userId);
        Task<Review?> GetReviewByIdAsync(string id);
        Task<Review> CreateReviewAsync(Review review);
        Task<Review?> UpdateReviewAsync(string id, Review review);
        Task<bool> DeleteReviewAsync(string id);
        Task UpdateBusinessRatingsAsync(string businessId);
    }
}
