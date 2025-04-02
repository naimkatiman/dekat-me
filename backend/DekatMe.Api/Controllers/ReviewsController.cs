using DekatMe.Api.Models;
using DekatMe.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DekatMe.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        
        // GET: api/reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }
        
        // GET: api/reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(string id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            
            if (review == null)
                return NotFound();
                
            return Ok(review);
        }
        
        // GET: api/reviews/business/5
        [HttpGet("business/{businessId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByBusiness(string businessId)
        {
            var reviews = await _reviewService.GetReviewsByBusinessIdAsync(businessId);
            return Ok(reviews);
        }
        
        // GET: api/reviews/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByUser(string userId)
        {
            var reviews = await _reviewService.GetReviewsByUserIdAsync(userId);
            return Ok(reviews);
        }
        
        // POST: api/reviews
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Review>> CreateReview(Review review)
        {
            // Set user ID from authenticated user if not explicitly set
            if (string.IsNullOrEmpty(review.UserId))
            {
                review.UserId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? string.Empty;
                
                if (string.IsNullOrEmpty(review.UserId))
                    return BadRequest("User ID is required");
            }
            
            var createdReview = await _reviewService.CreateReviewAsync(review);
            return CreatedAtAction(nameof(GetReview), new { id = createdReview.Id }, createdReview);
        }
        
        // PUT: api/reviews/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateReview(string id, Review review)
        {
            if (id != review.Id)
                return BadRequest();
                
            // Ensure the user is the owner of the review or an admin
            var existingReview = await _reviewService.GetReviewByIdAsync(id);
            if (existingReview == null)
                return NotFound();
                
            string userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? string.Empty;
            if (existingReview.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();
                
            var updatedReview = await _reviewService.UpdateReviewAsync(id, review);
            
            if (updatedReview == null)
                return NotFound();
                
            return NoContent();
        }
        
        // DELETE: api/reviews/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview(string id)
        {
            // Ensure the user is the owner of the review or an admin
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
                return NotFound();
                
            string userId = User.FindFirst("sub")?.Value ?? User.Identity?.Name ?? string.Empty;
            if (review.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();
                
            var result = await _reviewService.DeleteReviewAsync(id);
            
            if (!result)
                return NotFound();
                
            return NoContent();
        }
    }
}
