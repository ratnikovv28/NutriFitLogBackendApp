using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Nutrition;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Repositories.Nutrition;
using Xunit;

public class MealFoodRepositoryTests
{
    private readonly NutriFitLogContext _dbContext;
    private readonly MealFoodRepository _sut;

    public MealFoodRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<NutriFitLogContext>()
            .UseInMemoryDatabase($"{nameof(MealFoodRepositoryTests)}/{Guid.NewGuid()}")
            .Options;

        _dbContext = new NutriFitLogContext(dbContextOptions);
        _sut = new MealFoodRepository(_dbContext);
    }

    [Theory]
    [MemberData(nameof(MealsData))]
    public async Task GetById_WhenExists_ReturnsMealFood(MealFood mealFood)
    {
        // Arrange
        await _dbContext.MealFoods.AddAsync(mealFood);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetById(mealFood.Id);

        // Assert
        result.Should().BeEquivalentTo(mealFood);
    }

    [Theory]
    [AutoData]
    public async Task GetById_WhenNotExists_ReturnsNull(long mealFoodId)
    {
        // Arrange

        // Act
        var result = await _sut.GetById(mealFoodId);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(MealsData))]
    public async Task AddAsync_WhenCalled_AddsMealFood(MealFood mealFood)
    {
        // Arrange

        // Act
        var addedMealFood = await _sut.AddAsync(mealFood);
        await _dbContext.SaveChangesAsync();
        var result = await _dbContext.MealFoods.FindAsync(addedMealFood.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(mealFood);
    }

    [Theory]
    [MemberData(nameof(MealsData))]
    public async Task Delete_WhenCalled_DeletesMealFood(MealFood mealFood)
    {
        // Arrange
        await _dbContext.MealFoods.AddAsync(mealFood);
        await _dbContext.SaveChangesAsync();

        // Act
        _sut.Delete(mealFood);
        await _dbContext.SaveChangesAsync();
        var result = await _dbContext.MealFoods.FindAsync(mealFood.Id);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(MealsData))]
    public async Task Update_WhenCalled_UpdatesMealFood(MealFood mealFood)
    {
        // Arrange
        await _dbContext.MealFoods.AddAsync(mealFood);
        await _dbContext.SaveChangesAsync();

        mealFood.Quantity += 10;

        // Act
        var updatedMealFood = _sut.Update(mealFood);
        await _dbContext.SaveChangesAsync();
        var result = await _sut.GetById(mealFood.Id);

        // Assert
        result.Should().BeEquivalentTo(updatedMealFood);
    }
    
    [ExcludeFromCodeCoverage]
    public static IEnumerable<object[]> MealsData()
    {
        var fixture = new Fixture();

        var foods = new MealFood
        {
            Food = new Food { Name = "Food1" },
            Quantity = 100 + 10 * 1,
        };

        yield return new object[] { foods };
    }
}
