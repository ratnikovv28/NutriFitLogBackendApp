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