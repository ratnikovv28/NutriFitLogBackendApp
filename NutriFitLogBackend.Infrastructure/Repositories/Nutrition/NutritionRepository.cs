using NutriFitLogBackend.Domain.Entities.Nutrition;
using NutriFitLogBackend.Domain.Repositories.Nutrition;
using NutriFitLogBackend.Infrastructure.Database;

namespace NutriFitLogBackend.Infrastructure.Repositories.Nutrition;

public class NutritionRepository : INutritionRepository
{
    private readonly NutriFitLogContext _dbContext;

    public NutritionRepository(NutriFitLogContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Meal> AddAsync(Meal meal)
    {
        await _dbContext.Meals.AddAsync(meal);
        return meal;
    }
}