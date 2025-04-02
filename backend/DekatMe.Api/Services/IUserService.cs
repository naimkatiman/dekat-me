using DekatMe.Api.Models;

namespace DekatMe.Api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserProfileAsync(string id, ApplicationUser userProfile);
        Task<bool> AddBusinessToFavoritesAsync(string userId, string businessId);
        Task<bool> RemoveBusinessFromFavoritesAsync(string userId, string businessId);
        Task<IEnumerable<Business>> GetUserFavoriteBusinessesAsync(string userId);
    }
}
