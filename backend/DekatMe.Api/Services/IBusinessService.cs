using DekatMe.Api.Models;

namespace DekatMe.Api.Services
{
    public interface IBusinessService
    {
        Task<IEnumerable<Business>> GetAllBusinessesAsync();
        Task<Business?> GetBusinessByIdAsync(string id);
        Task<IEnumerable<Business>> GetBusinessesByCategoryIdAsync(string categoryId);
        Task<IEnumerable<Business>> SearchBusinessesAsync(string query);
        Task<IEnumerable<Business>> GetNearbyBusinessesAsync(double latitude, double longitude, double radiusKm);
        Task<Business> CreateBusinessAsync(Business business);
        Task<Business?> UpdateBusinessAsync(string id, Business business);
        Task<bool> DeleteBusinessAsync(string id);
        Task<bool> ToggleFeaturedStatusAsync(string id);
        Task<bool> TogglePremiumStatusAsync(string id);
        Task<bool> VerifyBusinessAsync(string id);
    }
}
