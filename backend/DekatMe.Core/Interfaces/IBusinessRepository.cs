using DekatMe.Core.Entities;

namespace DekatMe.Core.Interfaces
{
    public interface IBusinessRepository
    {
        Task<IEnumerable<Business>> GetAllAsync();
        Task<Business?> GetByIdAsync(string id);
        Task<IEnumerable<Business>> GetByCategoryIdAsync(string categoryId);
        Task<IEnumerable<Business>> SearchAsync(string query);
        Task<IEnumerable<Business>> GetNearbyAsync(double latitude, double longitude, double radiusKm);
        Task<IEnumerable<Business>> GetFeaturedAsync(int limit = 6);
        Task<Business> CreateAsync(Business business);
        Task<Business?> UpdateAsync(string id, Business business);
        Task<bool> DeleteAsync(string id);
        Task<bool> ToggleFeaturedStatusAsync(string id);
        Task<bool> TogglePremiumStatusAsync(string id);
        Task<bool> VerifyAsync(string id);
        Task<int> GetTotalCountAsync();
        Task<IEnumerable<Business>> GetPaginatedAsync(int page, int pageSize);
        Task<double> GetAverageRatingAsync();
        Task<IDictionary<string, int>> GetBusinessCountByCategoryAsync();
    }
}
