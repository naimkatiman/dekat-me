using DekatMe.Api.Models;
using DekatMe.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DekatMe.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessesController : ControllerBase
    {
        private readonly IBusinessService _businessService;

        public BusinessesController(IBusinessService businessService)
        {
            _businessService = businessService;
        }
        
        // GET: api/businesses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Business>>> GetBusinesses()
        {
            var businesses = await _businessService.GetAllBusinessesAsync();
            return Ok(businesses);
        }
        
        // GET: api/businesses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Business>> GetBusiness(string id)
        {
            var business = await _businessService.GetBusinessByIdAsync(id);
            
            if (business == null)
                return NotFound();
                
            return Ok(business);
        }
        
        // GET: api/businesses/category/5
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Business>>> GetBusinessesByCategory(string categoryId)
        {
            var businesses = await _businessService.GetBusinessesByCategoryIdAsync(categoryId);
            return Ok(businesses);
        }
        
        // GET: api/businesses/search?query=coffee
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Business>>> SearchBusinesses([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest("Search query cannot be empty");
                
            var businesses = await _businessService.SearchBusinessesAsync(query);
            return Ok(businesses);
        }
        
        // GET: api/businesses/nearby?lat=3.1390&lng=101.6869&radius=5
        [HttpGet("nearby")]
        public async Task<ActionResult<IEnumerable<Business>>> GetNearbyBusinesses(
            [FromQuery] double lat, 
            [FromQuery] double lng, 
            [FromQuery] double radius = 5)
        {
            var businesses = await _businessService.GetNearbyBusinessesAsync(lat, lng, radius);
            return Ok(businesses);
        }
        
        // POST: api/businesses
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Business>> CreateBusiness(Business business)
        {
            var createdBusiness = await _businessService.CreateBusinessAsync(business);
            return CreatedAtAction(nameof(GetBusiness), new { id = createdBusiness.Id }, createdBusiness);
        }
        
        // PUT: api/businesses/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBusiness(string id, Business business)
        {
            if (id != business.Id)
                return BadRequest();
                
            var updatedBusiness = await _businessService.UpdateBusinessAsync(id, business);
            
            if (updatedBusiness == null)
                return NotFound();
                
            return NoContent();
        }
        
        // DELETE: api/businesses/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBusiness(string id)
        {
            var result = await _businessService.DeleteBusinessAsync(id);
            
            if (!result)
                return NotFound();
                
            return NoContent();
        }
        
        // PATCH: api/businesses/5/feature
        [HttpPatch("{id}/feature")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleFeaturedStatus(string id)
        {
            var result = await _businessService.ToggleFeaturedStatusAsync(id);
            
            if (!result)
                return NotFound();
                
            return NoContent();
        }
        
        // PATCH: api/businesses/5/premium
        [HttpPatch("{id}/premium")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TogglePremiumStatus(string id)
        {
            var result = await _businessService.TogglePremiumStatusAsync(id);
            
            if (!result)
                return NotFound();
                
            return NoContent();
        }
        
        // PATCH: api/businesses/5/verify
        [HttpPatch("{id}/verify")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> VerifyBusiness(string id)
        {
            var result = await _businessService.VerifyBusinessAsync(id);
            
            if (!result)
                return NotFound();
                
            return NoContent();
        }
    }
}
