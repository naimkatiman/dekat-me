using DekatMe.Api.Models;
using DekatMe.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DekatMe.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        
        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }
        
        // GET: api/categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(string id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            
            if (category == null)
                return NotFound();
                
            return Ok(category);
        }
        
        // GET: api/categories/slug/food-beverage
        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<Category>> GetCategoryBySlug(string slug)
        {
            var category = await _categoryService.GetCategoryBySlugAsync(slug);
            
            if (category == null)
                return NotFound();
                
            return Ok(category);
        }
        
        // POST: api/categories
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            var createdCategory = await _categoryService.CreateCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
        }
        
        // PUT: api/categories/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(string id, Category category)
        {
            if (id != category.Id)
                return BadRequest();
                
            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, category);
            
            if (updatedCategory == null)
                return NotFound();
                
            return NoContent();
        }
        
        // DELETE: api/categories/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            
            if (!result)
                return NotFound();
                
            return NoContent();
        }
        
        // POST: api/categories/update-counts
        [HttpPost("update-counts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategoryCounts()
        {
            await _categoryService.UpdateCategoryCountsAsync();
            return NoContent();
        }
    }
}
