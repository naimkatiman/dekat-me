using DekatMe.Core.Entities;

namespace DekatMe.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> UpdateProfileAsync(string id, User userProfile);
        Task<bool> AddBusinessToFavoritesAsync(string userId, string businessId);
        Task<bool> RemoveBusinessFromFavoritesAsync(string userId, string businessId);
        Task<IEnumerable<Business>> GetFavoriteBusinessesAsync(string userId);
        Task<bool> IsBusinessFavoriteAsync(string userId, string businessId);
        Task<int> GetTotalUsersCountAsync();
        Task<User?> GetByUsernameAsync(string username);
        Task<IEnumerable<User>> GetTopReviewersAsync(int count);
        Task<IDictionary<string, int>> GetUserActivityStatisticsAsync(string userId);
    }
}
