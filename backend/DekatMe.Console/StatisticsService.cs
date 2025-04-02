using DekatMe.Core.Entities;
using Microsoft.Extensions.Logging;

namespace DekatMe.Console
{
    public class StatisticsService
    {
        private readonly ILogger<StatisticsService> _logger;

        public StatisticsService(ILogger<StatisticsService> logger)
        {
            _logger = logger;
        }

        public BusinessStatistics CalculateBusinessStatistics(IEnumerable<Business> businesses)
        {
            _logger.LogInformation("Calculating business statistics");

            var stats = new BusinessStatistics();
            
            if (!businesses.Any())
            {
                _logger.LogWarning("No businesses available for statistics calculation");
                return stats;
            }

            stats.TotalCount = businesses.Count();
            stats.FeaturedCount = businesses.Count(b => b.IsFeatured);
            stats.PremiumCount = businesses.Count(b => b.IsPremium);
            stats.VerifiedCount = businesses.Count(b => b.IsVerified);
            
            stats.AverageRating = businesses.Average(b => b.Rating);
            
            var byCategory = businesses
                .GroupBy(b => b.CategoryId)
                .Select(g => new CategoryDistribution
                {
                    CategoryId = g.Key,
                    CategoryName = g.First().Category?.Name ?? g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(c => c.Count)
                .ToList();
                
            stats.CategoryDistribution = byCategory;
            
            var byCity = businesses
                .GroupBy(b => b.City)
                .Select(g => new CityDistribution
                {
                    City = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(c => c.Count)
                .ToList();
                
            stats.CityDistribution = byCity;
            
            _logger.LogInformation("Statistics calculation completed");
            
            return stats;
        }

        public ReviewStatistics CalculateReviewStatistics(IEnumerable<Review> reviews)
        {
            _logger.LogInformation("Calculating review statistics");

            var stats = new ReviewStatistics();
            
            if (!reviews.Any())
            {
                _logger.LogWarning("No reviews available for statistics calculation");
                return stats;
            }

            stats.TotalCount = reviews.Count();
            stats.AverageRating = reviews.Average(r => r.Rating);
            
            var ratingDistribution = new int[5];
            foreach (var review in reviews)
            {
                if (review.Rating >= 1 && review.Rating <= 5)
                {
                    ratingDistribution[review.Rating - 1]++;
                }
            }
            
            stats.RatingDistribution = new Dictionary<int, int>
            {
                { 1, ratingDistribution[0] },
                { 2, ratingDistribution[1] },
                { 3, ratingDistribution[2] },
                { 4, ratingDistribution[3] },
                { 5, ratingDistribution[4] }
            };
            
            stats.ReviewsWithPhotos = reviews.Count(r => r.Photos.Any());
            
            var reviewsByDateGroup = reviews
                .GroupBy(r => r.CreatedAt.Date)
                .Select(g => new DateDistribution
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(d => d.Date)
                .ToList();
                
            stats.DateDistribution = reviewsByDateGroup;
            
            _logger.LogInformation("Review statistics calculation completed");
            
            return stats;
        }

        public UserStatistics CalculateUserStatistics(IEnumerable<User> users)
        {
            _logger.LogInformation("Calculating user statistics");

            var stats = new UserStatistics();
            
            if (!users.Any())
            {
                _logger.LogWarning("No users available for statistics calculation");
                return stats;
            }

            stats.TotalCount = users.Count();
            stats.ActiveUsersCount = users.Count(u => u.LastLogin.HasValue && (DateTime.UtcNow - u.LastLogin.Value).TotalDays <= 30);
            
            var usersByDate = users
                .GroupBy(u => u.CreatedAt.Date)
                .Select(g => new DateDistribution
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(d => d.Date)
                .ToList();
                
            stats.RegistrationDistribution = usersByDate;
            
            stats.UsersWithReviews = users.Count(u => u.Reviews.Any());
            stats.UsersWithFavorites = users.Count(u => u.FavoriteBusinesses.Any());
            
            var topReviewers = users
                .OrderByDescending(u => u.Reviews.Count)
                .Take(10)
                .Select(u => new TopReviewer
                {
                    UserId = u.Id,
                    UserName = u.UserName,
                    ReviewCount = u.Reviews.Count
                })
                .ToList();
                
            stats.TopReviewers = topReviewers;
            
            _logger.LogInformation("User statistics calculation completed");
            
            return stats;
        }
    }

    public class BusinessStatistics
    {
        public int TotalCount { get; set; }
        public int FeaturedCount { get; set; }
        public int PremiumCount { get; set; }
        public int VerifiedCount { get; set; }
        public double AverageRating { get; set; }
        public List<CategoryDistribution> CategoryDistribution { get; set; } = new List<CategoryDistribution>();
        public List<CityDistribution> CityDistribution { get; set; } = new List<CityDistribution>();
    }

    public class CategoryDistribution
    {
        public string CategoryId { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class CityDistribution
    {
        public string City { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class ReviewStatistics
    {
        public int TotalCount { get; set; }
        public double AverageRating { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new Dictionary<int, int>();
        public int ReviewsWithPhotos { get; set; }
        public List<DateDistribution> DateDistribution { get; set; } = new List<DateDistribution>();
    }

    public class UserStatistics
    {
        public int TotalCount { get; set; }
        public int ActiveUsersCount { get; set; }
        public List<DateDistribution> RegistrationDistribution { get; set; } = new List<DateDistribution>();
        public int UsersWithReviews { get; set; }
        public int UsersWithFavorites { get; set; }
        public List<TopReviewer> TopReviewers { get; set; } = new List<TopReviewer>();
    }

    public class DateDistribution
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    public class TopReviewer
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int ReviewCount { get; set; }
    }
}
