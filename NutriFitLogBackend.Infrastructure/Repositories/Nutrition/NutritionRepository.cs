using Microsoft.EntityFrameworkCore;
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

    public async Task<Meal> GetMealByIdAsync(long id)
    {
        return await _dbContext.Meals
            .Include(m => m.Foods)
            .ThenInclude(mf => mf.Food)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Meal>> GetAllMealsAsync()
    {
        return await _dbContext.Meals
            .Include(m => m.Foods)
            .ThenInclude(mf => mf.Food)
            .ToListAsync();
    }

    public async Task<Meal> AddMealAsync(Meal meal)
    {
        _dbContext.Meals.Add(meal);
        await _dbContext.SaveChangesAsync();
        return meal;
    }

    public async Task UpdateMealAsync(Meal meal)
    {
        _dbContext.Meals.Update(meal);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteMealAsync(Meal meal)
    {
        _dbContext.Meals.Remove(meal);
        await _dbContext.SaveChangesAsync();
    }
}