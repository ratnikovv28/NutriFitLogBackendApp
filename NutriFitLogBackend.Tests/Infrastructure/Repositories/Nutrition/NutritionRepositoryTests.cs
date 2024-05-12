using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Nutrition;
using NutriFitLogBackend.Domain.Entities.Users;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Repositories.Nutrition;
using Xunit;

namespace NutriFitLogBackend.Tests.Infrastructure.Repositories.Nutrition;

public class NutritionRepositoryTests
{
    private readonly NutriFitLogContext _dbContext;
    private readonly NutritionRepository _sut;

    public NutritionRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<NutriFitLogContext>()
            .UseInMemoryDatabase($"{nameof(NutritionRepositoryTests)}/{Guid.NewGuid()}")
            .Options;

        _dbContext = new NutriFitLogContext(dbContextOptions);
        _sut = new NutritionRepository(_dbContext);
    }
    
    [Theory]
    [MemberData(nameof(MealData))]
    public async Task Add_ReturnsCreatedMeal(Meal meal)
    {
        // Arrange
        
        // Act
        var addedMeal = await _sut.AddAsync(meal);
        await _dbContext.SaveChangesAsync();
        var foundMeal = await _sut.GetByIdAsync(addedMeal.Id);

        // Assert
        foundMeal.Should().NotBeNull();
        foundMeal.Should().BeEquivalentTo(meal, options => options
            .Excluding(m => m.Id)
            .Excluding(m => m.User.Id)
            .ExcludingNestedObjects());
    }
    
    [Theory]
    [MemberData(nameof(MealData))]
    public async Task GetAllByTelegramId_WhenMealsExist_ReturnsMeals(Meal meal)
    {
        // Arrange
        _dbContext.Meals.Add(meal);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllByTelegramIdAsync(meal.User.TelegramId);

        // Assert
        result.Should().NotBeEmpty();
        result.All(m => m.UserId == meal.UserId).Should().BeTrue();
    }
    
    [Theory]
    [MemberData(nameof(MealData))]
    public async Task GetAllByTelegramId_WhenNoMeals_ReturnsEmpty(Meal meal)
    {
        // Arrange

        // Act
        var result = await _sut.GetAllByTelegramIdAsync(meal.User.TelegramId); 

        // Assert
        result.Should().BeEmpty();
    }
    
    [Theory]
    [MemberData(nameof(MealData))]
    public async Task Update_UpdatesMealSuccessfully(Meal meal)
    {
        // Arrange
        _dbContext.Meals.Add(meal);
        await _dbContext.SaveChangesAsync();

        // Change some properties
        meal.Foods.First().Calories = 500; 

        // Act
        await _sut.UpdateAsync(meal);
        await _dbContext.SaveChangesAsync();

        // Assert
        var updatedMeal = await _sut.GetByIdAsync(meal.Id);
        updatedMeal.Foods.First().Calories.Should().Be(500);
    }
    
    [Theory]
    [MemberData(nameof(MealData))]
    public async Task Delete_DeletesMealSuccessfully(Meal meal)
    {
        // Arrange
        _dbContext.Meals.Add(meal);
        await _dbContext.SaveChangesAsync();

        // Act
        await _sut.DeleteAsync(meal);
        await _dbContext.SaveChangesAsync();

        // Assert
        var result = await _sut.GetByIdAsync(meal.Id);
        result.Should().BeNull();
    }
    
    [Theory]
    [MemberData(nameof(MealsData))]
    public async Task GetAllAsync_WhenMealsExist_ReturnsAllMeals(List<Meal> expectedMeals)
    {
        // Arrange
        _dbContext.Meals.AddRange(expectedMeals);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().HaveCount(expectedMeals.Count);
        result.Should().BeEquivalentTo(expectedMeals, options => options
            .ExcludingNestedObjects() 
            .Excluding(m => m.User));
    }
    
    [Fact]
    public async Task GetAllAsync_WhenNoMealsExist_ReturnsEmptyCollection()
    {
        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }
    
    public static IEnumerable<object[]> MealData()
    {
        var fixture = new Fixture();
        var telegramId = fixture.Create<long>();
        var user = new User(telegramId);
        var mealFood = new MealFood
        {
            Food = new Food { Name = "Food 1"},
            Quantity = 100 + 10,
            Unit = UnitOfMeasure.Grams
        };
        
        var meal = new Meal
        {
            CreatedDate = DateTime.UtcNow,
            User = user,
            Foods = new List<MealFood> { mealFood }
        };

        yield return new object[] { meal };
    }
    
    public static IEnumerable<object[]> MealsData()
    {
        var fixture = new Fixture();
        var meals = new List<Meal>();

        for (int i = 0; i < 3; i++)
        {
            var meal = new Meal
            {
                CreatedDate = DateTime.UtcNow.AddDays(-i),
                UserId = fixture.Create<long>(),
                Foods = new List<MealFood>
                {
                    new MealFood
                    {
                        Food = new Food { Name = "Food " + i },
                        Quantity = 100 + 10 * i,
                        Unit = UnitOfMeasure.Grams
                    }
                }
            };
            meals.Add(meal);
        }

        yield return new object[] { meals };
    }
}