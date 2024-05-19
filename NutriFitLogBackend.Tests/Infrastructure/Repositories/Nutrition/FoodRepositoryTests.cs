using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Nutrition;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Repositories.Nutrition;
using Xunit;

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
    public async Task GetAllAsync_WhenFoodsExist_ReturnsFoods()
    {
        // Arrange
        var foods = new List<Food>
        {
            new Food { Id = 1, Name = "Food1" },
            new Food { Id = 2, Name = "Food2" }
        };

        await _dbContext.Foods.AddRangeAsync(foods);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(foods);
    }

    [Fact]
    public async Task GetAllAsync_WhenFoodsNotExist_ReturnsEmptyCollection()
    {
        // Arrange

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(FoodData))]
    public async Task ExistAsync_WhenExists_ReturnsTrue(Food food)
    {
        // Arrange
        await _dbContext.Foods.AddAsync(food);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.ExistAsync(food.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [AutoData]
    public async Task ExistAsync_WhenNotExists_ReturnsFalse(long foodId)
    {
        // Arrange

        // Act
        var result = await _sut.ExistAsync(foodId);

        // Assert
        result.Should().BeFalse();
    }
    
    [ExcludeFromCodeCoverage]
    public static IEnumerable<object[]> FoodData()
    {
        var fixture = new Fixture();
        var food = new Food
        {
            Description = fixture.Create<string>(),
            Name = fixture.Create<string>(),
            PictureUrl = fixture.Create<string>(),
            Unit = UnitOfMeasure.Grams
        };

        yield return new object[]
        {
            food
        };
    }
}