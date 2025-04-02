using DekatMe.Api.Data;
using DekatMe.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DekatMe.Api.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users
                .OrderBy(u => u.UserName)
                .ToListAsync();
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> UpdateUserProfileAsync(string id, ApplicationUser userProfile)
        {
            var user = await _context.Users.FindAsync(id);
            
            if (user == null)
                return false;
                
            user.FirstName = userProfile.FirstName;
            user.LastName = userProfile.LastName;
            user.ProfilePicture = userProfile.ProfilePicture;
            user.PhoneNumber = userProfile.PhoneNumber;
            
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> AddBusinessToFavoritesAsync(string userId, string businessId)
        {
            var user = await _context.Users
                .Include(u => u.FavoriteBusinesses)
                .FirstOrDefaultAsync(u => u.Id == userId);
                
            var business = await _context.Businesses.FindAsync(businessId);
            
            if (user == null || business == null)
                return false;
                
            // Check if the business is already a favorite
            if (user.FavoriteBusinesses.Any(b => b.Id == businessId))
                return true; // Already a favorite
                
            user.FavoriteBusinesses.Add(business);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> RemoveBusinessFromFavoritesAsync(string userId, string businessId)
        {
            var user = await _context.Users
                .Include(u => u.FavoriteBusinesses)
                .FirstOrDefaultAsync(u => u.Id == userId);
                
            if (user == null)
                return false;
                
            var favorite = user.FavoriteBusinesses.FirstOrDefault(b => b.Id == businessId);
            
            if (favorite == null)
                return false; // Not a favorite
                
            user.FavoriteBusinesses.Remove(favorite);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<IEnumerable<Business>> GetUserFavoriteBusinessesAsync(string userId)
        {
            var user = await _context.Users
                .Include(u => u.FavoriteBusinesses)
                    .ThenInclude(b => b.Category)
                .Include(u => u.FavoriteBusinesses)
                    .ThenInclude(b => b.Images)
                .FirstOrDefaultAsync(u => u.Id == userId);
                
            if (user == null)
                return new List<Business>();
                
            return user.FavoriteBusinesses;
        }
    }
}
