using DekatMe.Api.Data;
using DekatMe.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DekatMe.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(string id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category?> GetCategoryBySlugAsync(string slug)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Slug == slug);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            
            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(string id, Category category)
        {
            var existingCategory = await _context.Categories.FindAsync(id);
            
            if (existingCategory == null)
                return null;
                
            existingCategory.Name = category.Name;
            existingCategory.Slug = category.Slug;
            existingCategory.Description = category.Description;
            existingCategory.Icon = category.Icon;
            
            await _context.SaveChangesAsync();
            
            return existingCategory;
        }

        public async Task<bool> DeleteCategoryAsync(string id)
        {
            var category = await _context.Categories.FindAsync(id);
            
            if (category == null)
                return false;
                
            // Check if there are any businesses in this category
            var hasBusinesses = await _context.Businesses
                .AnyAsync(b => b.CategoryId == id);
                
            if (hasBusinesses)
                return false; // Cannot delete a category that has businesses
                
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task UpdateCategoryCountsAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            
            foreach (var category in categories)
            {
                category.Count = await _context.Businesses
                    .CountAsync(b => b.CategoryId == category.Id);
            }
            
            await _context.SaveChangesAsync();
        }
    }
}
