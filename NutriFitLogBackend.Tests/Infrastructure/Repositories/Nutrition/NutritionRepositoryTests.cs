using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Nutrition;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Repositories.Nutrition;
using Xunit;

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
    public async Task AddAsync_WhenCalled_AddsMeal(Meal meal)
    {
        // Arrange

        // Act
        var addedMeal = await _sut.AddAsync(meal);
        await _dbContext.SaveChangesAsync();
        var result = await _dbContext.Meals.FindAsync(addedMeal.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(meal);
    }
    
    public static IEnumerable<object[]> MealData()
    {
        var fixture = new Fixture();

        var meal = new Meal
        {
            CreatedDate = DateTime.UtcNow.AddDays(-1),
            UserId = fixture.Create<long>(),
            Foods = new List<MealFood>
            {
                new MealFood
                {
                    Food = new Food { Name = "Food1" },
                    Quantity = 100 + 10 * 1,
                }
            }
        };

        yield return new object[] { meal };
    }
}