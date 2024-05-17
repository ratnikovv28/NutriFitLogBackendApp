using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Nutrition;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Repositories.Nutrition;
using Xunit;

namespace NutriFitLogBackend.Tests.Infrastructure.Repositories.Nutrition;

public class FoodRepositoryTests
{
    private readonly NutriFitLogContext _dbContext;
    private readonly FoodRepository _sut;

    public FoodRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<NutriFitLogContext>()
            .UseInMemoryDatabase($"{nameof(FoodRepositoryTests)}/{Guid.NewGuid()}")
            .Options;

        _dbContext = new NutriFitLogContext(dbContextOptions);
        _sut = new FoodRepository(_dbContext);
    }
    
    [Fact]
    public async Task GetAllAsync_WhenFoodsExists_ReturnsAllFoods()
    {
        // Arrange
        var foods = new List<Food>
        {
            new Food { Name = "Banana", Description = "Yellow banana",/* Calories = 89 */},
            new Food { Name = "Apple", Description = "Green apple",/* Calories = 52 */}
        };

        _dbContext.Foods.AddRange(foods);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().HaveCount(foods.Count);
        result.Should().BeEquivalentTo(foods);
    }
    
    [Fact]
    public async Task GetAllAsync_WhenFoodsNotExists_ReturnsEmptyCollection()
    {
        // Arrange
        
        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
        result.Count.Should().Be(0);
    }
}