using DekatMe.Core.Entities;

namespace DekatMe.Core.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllAsync();
        Task<IEnumerable<Review>> GetByBusinessIdAsync(string businessId);
        Task<IEnumerable<Review>> GetByUserIdAsync(string userId);
        Task<Review?> GetByIdAsync(string id);
        Task<Review> CreateAsync(Review review);
        Task<Review?> UpdateAsync(string id, Review review);
        Task<bool> DeleteAsync(string id);
        Task UpdateBusinessRatingsAsync(string businessId);
        Task<Dictionary<int, int>> GetRatingDistributionForBusinessAsync(string businessId);
        Task<double> GetAverageRatingAsync();
        Task<int> GetTotalReviewsCountAsync();
        Task<IEnumerable<Review>> GetRecentReviewsAsync(int count);
        Task<bool> HasUserReviewedBusinessAsync(string userId, string businessId);
    }
}
