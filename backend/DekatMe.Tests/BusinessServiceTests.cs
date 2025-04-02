using DekatMe.Api.Data;
using DekatMe.Api.Models;
using DekatMe.Api.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace DekatMe.Tests
{
    public class BusinessServiceTests
    {
        [Fact]
        public async Task GetAllBusinessesAsync_ReturnsAllBusinesses()
        {
            // Arrange
            var data = new List<Business>
            {
                new Business { Id = "1", Name = "Business 1" },
                new Business { Id = "2", Name = "Business 2" },
                new Business { Id = "3", Name = "Business 3" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Business>>();
            mockSet.As<IQueryable<Business>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Business>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Business>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Business>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Businesses).Returns(mockSet.Object);

            var service = new BusinessService(mockContext.Object);

            // Act
            var result = await service.GetAllBusinessesAsync();

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Equal("Business 1", result.First().Name);
            Assert.Equal("Business 3", result.Last().Name);
        }

        [Fact]
        public async Task GetBusinessByIdAsync_ReturnsCorrectBusiness()
        {
            // Arrange
            var testId = "2";
            var data = new List<Business>
            {
                new Business { Id = "1", Name = "Business 1" },
                new Business { Id = "2", Name = "Business 2" },
                new Business { Id = "3", Name = "Business 3" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Business>>();
            mockSet.As<IQueryable<Business>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Business>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Business>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Business>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) => data.FirstOrDefault(b => b.Id == (string)ids[0]));

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Businesses).Returns(mockSet.Object);

            var service = new BusinessService(mockContext.Object);

            // Act
            var result = await service.GetBusinessByIdAsync(testId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Business 2", result.Name);
        }

        [Fact]
        public async Task GetBusinessesByCategoryIdAsync_ReturnsCorrectBusinesses()
        {
            // Arrange
            var categoryId = "category1";
            var data = new List<Business>
            {
                new Business { Id = "1", Name = "Business 1", CategoryId = "category1" },
                new Business { Id = "2", Name = "Business 2", CategoryId = "category2" },
                new Business { Id = "3", Name = "Business 3", CategoryId = "category1" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Business>>();
            mockSet.As<IQueryable<Business>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Business>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Business>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Business>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Businesses).Returns(mockSet.Object);

            var service = new BusinessService(mockContext.Object);

            // Act
            var result = await service.GetBusinessesByCategoryIdAsync(categoryId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Business 1", result.First().Name);
            Assert.Equal("Business 3", result.Last().Name);
        }

        [Fact]
        public async Task SearchBusinessesAsync_ReturnsMatchingBusinesses()
        {
            // Arrange
            var query = "coffee";
            var data = new List<Business>
            {
                new Business { Id = "1", Name = "Coffee Shop", Description = "Best coffee in town" },
                new Business { Id = "2", Name = "Restaurant", Description = "Fine dining" },
                new Business { Id = "3", Name = "Bakery", Description = "Fresh bread and coffee" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Business>>();
            mockSet.As<IQueryable<Business>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Business>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Business>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Business>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Businesses).Returns(mockSet.Object);

            var service = new BusinessService(mockContext.Object);

            // Act
            var result = await service.SearchBusinessesAsync(query);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Coffee Shop", result.First().Name);
            Assert.Equal("Bakery", result.Last().Name);
        }

        [Fact]
        public async Task CreateBusinessAsync_AddsBusinessToContext()
        {
            // Arrange
            var business = new Business { Id = "1", Name = "New Business" };

            var mockSet = new Mock<DbSet<Business>>();
            mockSet.Setup(m => m.Add(It.IsAny<Business>())).Callback<Business>(b => Assert.Equal(business, b));

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Businesses).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new BusinessService(mockContext.Object);

            // Act
            var result = await service.CreateBusinessAsync(business);

            // Assert
            mockSet.Verify(m => m.Add(It.IsAny<Business>()), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(business, result);
        }

        [Fact]
        public async Task UpdateBusinessAsync_UpdatesExistingBusiness()
        {
            // Arrange
            var id = "1";
            var existingBusiness = new Business 
            { 
                Id = id, 
                Name = "Original Name", 
                Description = "Original Description",
                Phone = "Original Phone"
            };
            
            var updatedBusiness = new Business 
            { 
                Id = id, 
                Name = "Updated Name", 
                Description = "Updated Description",
                Phone = "Updated Phone"
            };

            var mockSet = new Mock<DbSet<Business>>();
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(existingBusiness);

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Businesses).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new BusinessService(mockContext.Object);

            // Act
            var result = await service.UpdateBusinessAsync(id, updatedBusiness);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Name", existingBusiness.Name);
            Assert.Equal("Updated Description", existingBusiness.Description);
            Assert.Equal("Updated Phone", existingBusiness.Phone);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteBusinessAsync_RemovesBusinessFromContext()
        {
            // Arrange
            var id = "1";
            var business = new Business { Id = id, Name = "Business to Delete" };

            var mockSet = new Mock<DbSet<Business>>();
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(business);
            mockSet.Setup(m => m.Remove(It.IsAny<Business>())).Callback<Business>(b => Assert.Equal(business, b));

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Businesses).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new BusinessService(mockContext.Object);

            // Act
            var result = await service.DeleteBusinessAsync(id);

            // Assert
            Assert.True(result);
            mockSet.Verify(m => m.Remove(It.IsAny<Business>()), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
