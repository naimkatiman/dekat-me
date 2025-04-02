using DekatMe.Core.Entities;

namespace DekatMe.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(string id);
        Task<Category?> GetBySlugAsync(string slug);
        Task<Category> CreateAsync(Category category);
        Task<Category?> UpdateAsync(string id, Category category);
        Task<bool> DeleteAsync(string id);
        Task UpdateCategoryCountsAsync();
        Task<bool> SlugExistsAsync(string slug);
        Task<IEnumerable<Category>> GetCategoriesWithBusinessCountAsync();
        Task<Dictionary<string, int>> GetCategoryDistributionAsync();
    }
}
