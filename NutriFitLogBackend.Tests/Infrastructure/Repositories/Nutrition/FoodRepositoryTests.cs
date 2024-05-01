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
    public async Task AddAsync_AddsFoodSuccessfully()
    {
        // Arrange
        var food = new Food
        {
            Name = "Apple",
            Description = "Fresh apples",
            /*Calories = 95,
            Protein = 0.5,
            Fats = 0.3,
            Carbohydrates = 25*/
        };

        // Act
        var addedFood = await _sut.AddAsync(food);
        await _dbContext.SaveChangesAsync();  // Ensure changes are saved

        // Assert
        var foundFood = await _sut.GetByIdAsync(addedFood.Id);
        foundFood.Should().BeEquivalentTo(food, options => options.Excluding(f => f.Id));
    }
    
    [Fact]
    public async Task GetByIdAsync_WhenFoodExists_ReturnsCorrectFood()
    {
        // Arrange
        var food = new Food
        {
            Name = "Banana",
            Description = "Ripe bananas",
            /*Calories = 105,
            Protein = 1.1,
            Fats = 0.3,
            Carbohydrates = 27*/
        };

        await _dbContext.Foods.AddAsync(food);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(food.Id);

        // Assert
        result.Should().BeEquivalentTo(food);
    }
    
    [Fact]
    public async Task GetByIdAsync_WhenFoodNotExists_ReturnsNull()
    {
        // Arrange
        var food = new Food
        {
            Name = "Banana",
            Description = "Ripe bananas",
            /*Calories = 105,
            Protein = 1.1,
            Fats = 0.3,
            Carbohydrates = 27*/
        };

        // Act
        var result = await _sut.GetByIdAsync(food.Id);

        // Assert
        result.Should().BeNull();
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
    
    [Fact]
    public async Task UpdateAsync_UpdatesFoodCorrectly()
    {
        // Arrange
        var food = new Food
        {
            Name = "Strawberry",
            Description = "Fresh strawberries",
            /*Calories = 32*/
        };

        _dbContext.Foods.Add(food);
        await _dbContext.SaveChangesAsync();

        // Update some properties
        /*food.Calories = 31;*/
        food.Description = "Very fresh strawberries";

        // Act
        await _sut.UpdateAsync(food);
        await _dbContext.SaveChangesAsync();

        // Assert
        var updatedFood = await _sut.GetByIdAsync(food.Id);
        /*updatedFood.Calories.Should().Be(31);*/
        updatedFood.Description.Should().Be("Very fresh strawberries");
    }
    
    [Fact]
    public async Task DeleteAsync_DeletesFoodCorrectly()
    {
        // Arrange
        var food = new Food
        {
            Name = "Orange",
            Description = "Juicy orange",
            /*Calories = 47*/
        };

        _dbContext.Foods.Add(food);
        await _dbContext.SaveChangesAsync();

        // Act
        await _sut.DeleteAsync(food);
        await _dbContext.SaveChangesAsync();

        // Assert
        var result = await _sut.GetByIdAsync(food.Id);
        result.Should().BeNull();
    }
}