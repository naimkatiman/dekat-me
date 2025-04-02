using DekatMe.Api.Data;
using DekatMe.Api.Models;
using DekatMe.Api.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace DekatMe.Tests
{
    public class ReviewServiceTests
    {
        [Fact]
        public async Task GetAllReviewsAsync_ReturnsAllReviews()
        {
            // Arrange
            var data = new List<Review>
            {
                new Review { Id = "1", BusinessId = "1", UserId = "1", Rating = 5, Comment = "Great place!" },
                new Review { Id = "2", BusinessId = "1", UserId = "2", Rating = 4, Comment = "Good service." },
                new Review { Id = "3", BusinessId = "2", UserId = "1", Rating = 3, Comment = "Average experience." }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Review>>();
            mockSet.As<IQueryable<Review>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Review>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Review>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Review>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Reviews).Returns(mockSet.Object);

            var service = new ReviewService(mockContext.Object);

            // Act
            var result = await service.GetAllReviewsAsync();

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Contains(result, r => r.Id == "1");
            Assert.Contains(result, r => r.Id == "2");
            Assert.Contains(result, r => r.Id == "3");
        }

        [Fact]
        public async Task GetReviewsByBusinessIdAsync_ReturnsCorrectReviews()
        {
            // Arrange
            var businessId = "1";
            var data = new List<Review>
            {
                new Review { Id = "1", BusinessId = "1", UserId = "1", Rating = 5, Comment = "Great place!" },
                new Review { Id = "2", BusinessId = "1", UserId = "2", Rating = 4, Comment = "Good service." },
                new Review { Id = "3", BusinessId = "2", UserId = "1", Rating = 3, Comment = "Average experience." }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Review>>();
            mockSet.As<IQueryable<Review>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Review>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Review>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Review>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Reviews).Returns(mockSet.Object);

            var service = new ReviewService(mockContext.Object);

            // Act
            var result = await service.GetReviewsByBusinessIdAsync(businessId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, r => Assert.Equal(businessId, r.BusinessId));
        }

        [Fact]
        public async Task GetReviewsByUserIdAsync_ReturnsCorrectReviews()
        {
            // Arrange
            var userId = "1";
            var data = new List<Review>
            {
                new Review { Id = "1", BusinessId = "1", UserId = "1", Rating = 5, Comment = "Great place!" },
                new Review { Id = "2", BusinessId = "1", UserId = "2", Rating = 4, Comment = "Good service." },
                new Review { Id = "3", BusinessId = "2", UserId = "1", Rating = 3, Comment = "Average experience." }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Review>>();
            mockSet.As<IQueryable<Review>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Review>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Review>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Review>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Reviews).Returns(mockSet.Object);

            var service = new ReviewService(mockContext.Object);

            // Act
            var result = await service.GetReviewsByUserIdAsync(userId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, r => Assert.Equal(userId, r.UserId));
        }

        [Fact]
        public async Task GetReviewByIdAsync_ReturnsCorrectReview()
        {
            // Arrange
            var id = "2";
            var data = new List<Review>
            {
                new Review { Id = "1", BusinessId = "1", UserId = "1", Rating = 5, Comment = "Great place!" },
                new Review { Id = "2", BusinessId = "1", UserId = "2", Rating = 4, Comment = "Good service." },
                new Review { Id = "3", BusinessId = "2", UserId = "1", Rating = 3, Comment = "Average experience." }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Review>>();
            mockSet.As<IQueryable<Review>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Review>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Review>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Review>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Reviews).Returns(mockSet.Object);

            var service = new ReviewService(mockContext.Object);

            // Act
            var result = await service.GetReviewByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("2", result.Id);
            Assert.Equal("Good service.", result.Comment);
            Assert.Equal(4, result.Rating);
        }

        [Fact]
        public async Task CreateReviewAsync_AddsReviewToContext_AndUpdatesBusinessRating()
        {
            // Arrange
            var review = new Review 
            { 
                Id = "4", 
                BusinessId = "1", 
                UserId = "3", 
                Rating = 5, 
                Comment = "Excellent!" 
            };

            var business = new Business { Id = "1", Name = "Test Business" };

            var mockReviewSet = new Mock<DbSet<Review>>();
            mockReviewSet.Setup(m => m.Add(It.IsAny<Review>())).Callback<Review>(r => Assert.Equal(review, r));

            var mockBusinessSet = new Mock<DbSet<Business>>();
            mockBusinessSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(business);

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Reviews).Returns(mockReviewSet.Object);
            mockContext.Setup(c => c.Businesses).Returns(mockBusinessSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new ReviewService(mockContext.Object);

            // Act
            var result = await service.CreateReviewAsync(review);

            // Assert
            mockReviewSet.Verify(m => m.Add(It.IsAny<Review>()), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeast(1));
            Assert.Equal(review, result);
        }

        [Fact]
        public async Task UpdateReviewAsync_UpdatesExistingReview_AndUpdatesBusinessRating()
        {
            // Arrange
            var id = "2";
            var existingReview = new Review 
            { 
                Id = id, 
                BusinessId = "1", 
                UserId = "2", 
                Rating = 3, 
                Comment = "Original Comment" 
            };
            
            var updatedReview = new Review 
            { 
                Id = id, 
                BusinessId = "1", 
                UserId = "2", 
                Rating = 5, 
                Comment = "Updated Comment" 
            };

            var business = new Business { Id = "1", Name = "Test Business" };

            var mockReviewSet = new Mock<DbSet<Review>>();
            mockReviewSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(existingReview);

            var mockBusinessSet = new Mock<DbSet<Business>>();
            mockBusinessSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(business);

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Reviews).Returns(mockReviewSet.Object);
            mockContext.Setup(c => c.Businesses).Returns(mockBusinessSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new ReviewService(mockContext.Object);

            // Act
            var result = await service.UpdateReviewAsync(id, updatedReview);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, existingReview.Rating);
            Assert.Equal("Updated Comment", existingReview.Comment);
            Assert.NotNull(existingReview.UpdatedAt);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeast(1));
        }

        [Fact]
        public async Task DeleteReviewAsync_RemovesReviewFromContext_AndUpdatesBusinessRating()
        {
            // Arrange
            var id = "2";
            var review = new Review 
            { 
                Id = id, 
                BusinessId = "1", 
                UserId = "2", 
                Rating = 4, 
                Comment = "Good service." 
            };

            var business = new Business { Id = "1", Name = "Test Business" };

            var mockReviewSet = new Mock<DbSet<Review>>();
            mockReviewSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(review);
            mockReviewSet.Setup(m => m.Remove(It.IsAny<Review>())).Callback<Review>(r => Assert.Equal(review, r));

            var mockBusinessSet = new Mock<DbSet<Business>>();
            mockBusinessSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(business);

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Reviews).Returns(mockReviewSet.Object);
            mockContext.Setup(c => c.Businesses).Returns(mockBusinessSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new ReviewService(mockContext.Object);

            // Act
            var result = await service.DeleteReviewAsync(id);

            // Assert
            Assert.True(result);
            mockReviewSet.Verify(m => m.Remove(It.IsAny<Review>()), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeast(1));
        }

        [Fact]
        public async Task UpdateBusinessRatingsAsync_CalculatesCorrectAverageAndCount()
        {
            // Arrange
            var businessId = "1";
            var business = new Business 
            { 
                Id = businessId, 
                Name = "Test Business", 
                Rating = 0, 
                ReviewsCount = 0 
            };

            var reviews = new List<Review>
            {
                new Review { Id = "1", BusinessId = businessId, Rating = 5 },
                new Review { Id = "2", BusinessId = businessId, Rating = 4 },
                new Review { Id = "3", BusinessId = businessId, Rating = 3 }
            }.AsQueryable();

            var mockReviewSet = new Mock<DbSet<Review>>();
            mockReviewSet.As<IQueryable<Review>>().Setup(m => m.Provider).Returns(reviews.Provider);
            mockReviewSet.As<IQueryable<Review>>().Setup(m => m.Expression).Returns(reviews.Expression);
            mockReviewSet.As<IQueryable<Review>>().Setup(m => m.ElementType).Returns(reviews.ElementType);
            mockReviewSet.As<IQueryable<Review>>().Setup(m => m.GetEnumerator()).Returns(reviews.GetEnumerator());

            var mockBusinessSet = new Mock<DbSet<Business>>();
            mockBusinessSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(business);

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Reviews).Returns(mockReviewSet.Object);
            mockContext.Setup(c => c.Businesses).Returns(mockBusinessSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new ReviewService(mockContext.Object);

            // Act
            await service.UpdateBusinessRatingsAsync(businessId);

            // Assert
            Assert.Equal(3, business.ReviewsCount);
            Assert.Equal(4.0, business.Rating); // (5 + 4 + 3) / 3 = 4.0
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateBusinessRatingsAsync_SetsZeroRatingWhenNoReviews()
        {
            // Arrange
            var businessId = "1";
            var business = new Business 
            { 
                Id = businessId, 
                Name = "Test Business", 
                Rating = 4.5, 
                ReviewsCount = 10 
            };

            var reviews = new List<Review>().AsQueryable();

            var mockReviewSet = new Mock<DbSet<Review>>();
            mockReviewSet.As<IQueryable<Review>>().Setup(m => m.Provider).Returns(reviews.Provider);
            mockReviewSet.As<IQueryable<Review>>().Setup(m => m.Expression).Returns(reviews.Expression);
            mockReviewSet.As<IQueryable<Review>>().Setup(m => m.ElementType).Returns(reviews.ElementType);
            mockReviewSet.As<IQueryable<Review>>().Setup(m => m.GetEnumerator()).Returns(reviews.GetEnumerator());

            var mockBusinessSet = new Mock<DbSet<Business>>();
            mockBusinessSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(business);

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Reviews).Returns(mockReviewSet.Object);
            mockContext.Setup(c => c.Businesses).Returns(mockBusinessSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new ReviewService(mockContext.Object);

            // Act
            await service.UpdateBusinessRatingsAsync(businessId);

            // Assert
            Assert.Equal(0, business.ReviewsCount);
            Assert.Equal(0, business.Rating);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
