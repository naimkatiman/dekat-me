using DekatMe.Api.Data;
using DekatMe.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DekatMe.Api.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Business)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByBusinessIdAsync(string businessId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.BusinessId == businessId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(string userId)
        {
            return await _context.Reviews
                .Include(r => r.Business)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Review?> GetReviewByIdAsync(string id)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Business)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Review> CreateReviewAsync(Review review)
        {
            review.CreatedAt = DateTime.UtcNow;
            
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            
            // Update business rating
            await UpdateBusinessRatingsAsync(review.BusinessId);
            
            return review;
        }

        public async Task<Review?> UpdateReviewAsync(string id, Review review)
        {
            var existingReview = await _context.Reviews.FindAsync(id);
            
            if (existingReview == null)
                return null;
                
            existingReview.Rating = review.Rating;
            existingReview.Comment = review.Comment;
            existingReview.Photos = review.Photos;
            existingReview.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            
            // Update business rating
            await UpdateBusinessRatingsAsync(existingReview.BusinessId);
            
            return existingReview;
        }

        public async Task<bool> DeleteReviewAsync(string id)
        {
            var review = await _context.Reviews.FindAsync(id);
            
            if (review == null)
                return false;
            
            string businessId = review.BusinessId; // Save for rating update
                
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            
            // Update business rating
            await UpdateBusinessRatingsAsync(businessId);
            
            return true;
        }

        public async Task UpdateBusinessRatingsAsync(string businessId)
        {
            var business = await _context.Businesses.FindAsync(businessId);
            
            if (business == null)
                return;
            
            var reviews = await _context.Reviews
                .Where(r => r.BusinessId == businessId)
                .ToListAsync();
            
            business.ReviewsCount = reviews.Count;
            
            if (reviews.Any())
                business.Rating = reviews.Average(r => r.Rating);
            else
                business.Rating = 0;
            
            business.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
        }
    }
}
