using DekatMe.Api.Data;
using DekatMe.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DekatMe.Api.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly ApplicationDbContext _context;

        public BusinessService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Business>> GetAllBusinessesAsync()
        {
            return await _context.Businesses
                .Include(b => b.Category)
                .Include(b => b.Images)
                .Include(b => b.Hours)
                .ToListAsync();
        }

        public async Task<Business?> GetBusinessByIdAsync(string id)
        {
            return await _context.Businesses
                .Include(b => b.Category)
                .Include(b => b.Images)
                .Include(b => b.Hours)
                .Include(b => b.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Business>> GetBusinessesByCategoryIdAsync(string categoryId)
        {
            return await _context.Businesses
                .Include(b => b.Category)
                .Include(b => b.Images)
                .Where(b => b.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Business>> SearchBusinessesAsync(string query)
        {
            return await _context.Businesses
                .Include(b => b.Category)
                .Include(b => b.Images)
                .Where(b => b.Name.Contains(query) || 
                            b.Description.Contains(query) || 
                            b.ShortDescription.Contains(query) ||
                            b.Address.Contains(query) ||
                            b.City.Contains(query))
                .ToListAsync();
        }

        public async Task<IEnumerable<Business>> GetNearbyBusinessesAsync(double latitude, double longitude, double radiusKm)
        {
            // Using simplified distance calculation
            // For more accurate results, consider using a geospatial library or SQL spatial functions
            const double earthRadiusKm = 6371;
            
            var businesses = await _context.Businesses
                .Include(b => b.Category)
                .Include(b => b.Images)
                .ToListAsync();
                
            return businesses.Where(b => 
            {
                var dLat = DegreesToRadians(b.Latitude - latitude);
                var dLon = DegreesToRadians(b.Longitude - longitude);
                
                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Cos(DegreesToRadians(latitude)) * Math.Cos(DegreesToRadians(b.Latitude)) *
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                
                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                var distance = earthRadiusKm * c;
                
                return distance <= radiusKm;
            });
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        public async Task<Business> CreateBusinessAsync(Business business)
        {
            business.CreatedAt = DateTime.UtcNow;
            business.UpdatedAt = DateTime.UtcNow;
            
            _context.Businesses.Add(business);
            await _context.SaveChangesAsync();
            
            return business;
        }

        public async Task<Business?> UpdateBusinessAsync(string id, Business business)
        {
            var existingBusiness = await _context.Businesses.FindAsync(id);
            
            if (existingBusiness == null)
                return null;
                
            // Update properties
            existingBusiness.Name = business.Name;
            existingBusiness.Description = business.Description;
            existingBusiness.ShortDescription = business.ShortDescription;
            existingBusiness.Address = business.Address;
            existingBusiness.City = business.City;
            existingBusiness.PostalCode = business.PostalCode;
            existingBusiness.CategoryId = business.CategoryId;
            existingBusiness.Latitude = business.Latitude;
            existingBusiness.Longitude = business.Longitude;
            existingBusiness.Phone = business.Phone;
            existingBusiness.Email = business.Email;
            existingBusiness.Website = business.Website;
            existingBusiness.Tags = business.Tags;
            existingBusiness.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            
            return existingBusiness;
        }

        public async Task<bool> DeleteBusinessAsync(string id)
        {
            var business = await _context.Businesses.FindAsync(id);
            
            if (business == null)
                return false;
                
            _context.Businesses.Remove(business);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> ToggleFeaturedStatusAsync(string id)
        {
            var business = await _context.Businesses.FindAsync(id);
            
            if (business == null)
                return false;
                
            business.IsFeatured = !business.IsFeatured;
            business.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> TogglePremiumStatusAsync(string id)
        {
            var business = await _context.Businesses.FindAsync(id);
            
            if (business == null)
                return false;
                
            business.IsPremium = !business.IsPremium;
            business.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> VerifyBusinessAsync(string id)
        {
            var business = await _context.Businesses.FindAsync(id);
            
            if (business == null)
                return false;
                
            business.IsVerified = true;
            business.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}
