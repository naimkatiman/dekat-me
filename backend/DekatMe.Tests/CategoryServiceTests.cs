using DekatMe.Api.Data;
using DekatMe.Api.Models;
using DekatMe.Api.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace DekatMe.Tests
{
    public class CategoryServiceTests
    {
        [Fact]
        public async Task GetAllCategoriesAsync_ReturnsAllCategories()
        {
            // Arrange
            var data = new List<Category>
            {
                new Category { Id = "1", Name = "Food & Beverage", Slug = "food-beverage" },
                new Category { Id = "2", Name = "Shopping", Slug = "shopping" },
                new Category { Id = "3", Name = "Services", Slug = "services" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Category>>();
            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Categories).Returns(mockSet.Object);

            var service = new CategoryService(mockContext.Object);

            // Act
            var result = await service.GetAllCategoriesAsync();

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Contains(result, c => c.Name == "Food & Beverage");
            Assert.Contains(result, c => c.Name == "Shopping");
            Assert.Contains(result, c => c.Name == "Services");
        }

        [Fact]
        public async Task GetCategoryByIdAsync_ReturnsCorrectCategory()
        {
            // Arrange
            var testId = "2";
            var data = new List<Category>
            {
                new Category { Id = "1", Name = "Food & Beverage", Slug = "food-beverage" },
                new Category { Id = "2", Name = "Shopping", Slug = "shopping" },
                new Category { Id = "3", Name = "Services", Slug = "services" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Category>>();
            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Categories).Returns(mockSet.Object);

            var service = new CategoryService(mockContext.Object);

            // Act
            var result = await service.GetCategoryByIdAsync(testId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Shopping", result.Name);
            Assert.Equal("shopping", result.Slug);
        }

        [Fact]
        public async Task GetCategoryBySlugAsync_ReturnsCorrectCategory()
        {
            // Arrange
            var testSlug = "shopping";
            var data = new List<Category>
            {
                new Category { Id = "1", Name = "Food & Beverage", Slug = "food-beverage" },
                new Category { Id = "2", Name = "Shopping", Slug = "shopping" },
                new Category { Id = "3", Name = "Services", Slug = "services" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Category>>();
            mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Categories).Returns(mockSet.Object);

            var service = new CategoryService(mockContext.Object);

            // Act
            var result = await service.GetCategoryBySlugAsync(testSlug);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Shopping", result.Name);
            Assert.Equal("2", result.Id);
        }

        [Fact]
        public async Task CreateCategoryAsync_AddsCategoryToContext()
        {
            // Arrange
            var category = new Category { Id = "4", Name = "Technology", Slug = "technology" };

            var mockSet = new Mock<DbSet<Category>>();
            mockSet.Setup(m => m.Add(It.IsAny<Category>())).Callback<Category>(c => Assert.Equal(category, c));

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Categories).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new CategoryService(mockContext.Object);

            // Act
            var result = await service.CreateCategoryAsync(category);

            // Assert
            mockSet.Verify(m => m.Add(It.IsAny<Category>()), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(category, result);
        }

        [Fact]
        public async Task UpdateCategoryAsync_UpdatesExistingCategory()
        {
            // Arrange
            var id = "2";
            var existingCategory = new Category 
            { 
                Id = id, 
                Name = "Original Name", 
                Slug = "original-slug",
                Description = "Original Description"
            };
            
            var updatedCategory = new Category 
            { 
                Id = id, 
                Name = "Updated Name", 
                Slug = "updated-slug",
                Description = "Updated Description"
            };

            var mockSet = new Mock<DbSet<Category>>();
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(existingCategory);

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Categories).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new CategoryService(mockContext.Object);

            // Act
            var result = await service.UpdateCategoryAsync(id, updatedCategory);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Name", existingCategory.Name);
            Assert.Equal("updated-slug", existingCategory.Slug);
            Assert.Equal("Updated Description", existingCategory.Description);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCategoryAsync_RemovesCategoryFromContext_WhenNoBusinessesExist()
        {
            // Arrange
            var id = "2";
            var category = new Category { Id = id, Name = "Category to Delete" };

            var mockCategorySet = new Mock<DbSet<Category>>();
            mockCategorySet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(category);
            mockCategorySet.Setup(m => m.Remove(It.IsAny<Category>())).Callback<Category>(c => Assert.Equal(category, c));

            // Mock empty business set (no businesses in this category)
            var mockBusinessSet = new Mock<DbSet<Business>>();
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.Provider).Returns((new List<Business>()).AsQueryable().Provider);
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.Expression).Returns((new List<Business>()).AsQueryable().Expression);
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.ElementType).Returns((new List<Business>()).AsQueryable().ElementType);
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.GetEnumerator()).Returns((new List<Business>()).AsQueryable().GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Categories).Returns(mockCategorySet.Object);
            mockContext.Setup(c => c.Businesses).Returns(mockBusinessSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new CategoryService(mockContext.Object);

            // Act
            var result = await service.DeleteCategoryAsync(id);

            // Assert
            Assert.True(result);
            mockCategorySet.Verify(m => m.Remove(It.IsAny<Category>()), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCategoryAsync_ReturnsFalse_WhenBusinessesExist()
        {
            // Arrange
            var id = "2";
            var category = new Category { Id = id, Name = "Category with Businesses" };

            var mockCategorySet = new Mock<DbSet<Category>>();
            mockCategorySet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(category);

            // Mock business set with businesses in this category
            var businesses = new List<Business>
            {
                new Business { Id = "1", Name = "Business 1", CategoryId = id }
            }.AsQueryable();

            var mockBusinessSet = new Mock<DbSet<Business>>();
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.Provider).Returns(businesses.Provider);
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.Expression).Returns(businesses.Expression);
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.ElementType).Returns(businesses.ElementType);
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.GetEnumerator()).Returns(businesses.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Categories).Returns(mockCategorySet.Object);
            mockContext.Setup(c => c.Businesses).Returns(mockBusinessSet.Object);

            var service = new CategoryService(mockContext.Object);

            // Act
            var result = await service.DeleteCategoryAsync(id);

            // Assert
            Assert.False(result);
            mockCategorySet.Verify(m => m.Remove(It.IsAny<Category>()), Times.Never);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCategoryCountsAsync_UpdatesCountForAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = "1", Name = "Category 1", Count = 0 },
                new Category { Id = "2", Name = "Category 2", Count = 0 }
            };
            
            var businesses = new List<Business>
            {
                new Business { Id = "1", Name = "Business 1", CategoryId = "1" },
                new Business { Id = "2", Name = "Business 2", CategoryId = "1" },
                new Business { Id = "3", Name = "Business 3", CategoryId = "2" }
            };

            var mockCategorySet = new Mock<DbSet<Category>>();
            mockCategorySet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(categories.AsQueryable().Provider);
            mockCategorySet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(categories.AsQueryable().Expression);
            mockCategorySet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(categories.AsQueryable().ElementType);
            mockCategorySet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(categories.AsQueryable().GetEnumerator());

            var mockBusinessSet = new Mock<DbSet<Business>>();
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.Provider).Returns(businesses.AsQueryable().Provider);
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.Expression).Returns(businesses.AsQueryable().Expression);
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.ElementType).Returns(businesses.AsQueryable().ElementType);
            mockBusinessSet.As<IQueryable<Business>>().Setup(m => m.GetEnumerator()).Returns(businesses.AsQueryable().GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Categories).Returns(mockCategorySet.Object);
            mockContext.Setup(c => c.Businesses).Returns(mockBusinessSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new CategoryService(mockContext.Object);

            // Act
            await service.UpdateCategoryCountsAsync();

            // Assert
            Assert.Equal(2, categories[0].Count); // Category 1 has 2 businesses
            Assert.Equal(1, categories[1].Count); // Category 2 has 1 business
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
