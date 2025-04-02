using DekatMe.Api.Models;

namespace DekatMe.Api.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(string id);
        Task<Category?> GetCategoryBySlugAsync(string slug);
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category?> UpdateCategoryAsync(string id, Category category);
        Task<bool> DeleteCategoryAsync(string id);
        Task UpdateCategoryCountsAsync();
    }
}
