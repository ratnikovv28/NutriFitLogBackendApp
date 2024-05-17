using NutriFitLogBackend.Domain.Entities.Nutrition;
using NutriFitLogBackend.Domain.Repositories.Nutrition;
using NutriFitLogBackend.Infrastructure.Database;

namespace NutriFitLogBackend.Infrastructure.Repositories.Nutrition;

public class MealFoodRepository : IMealFoodRepository
{
    private readonly NutriFitLogContext _dbContext;

    public MealFoodRepository(NutriFitLogContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<MealFood> GetById(long mealFoodId)
    {
        return await _dbContext.MealFoods.FindAsync(mealFoodId);
    }

    
    public async Task<MealFood> AddAsync(MealFood mealFood)
    {
        await _dbContext.MealFoods.AddAsync(mealFood);
        return mealFood;
    }

    public void Delete(MealFood mealFood)
    {
        _dbContext.MealFoods.Remove(mealFood);
    }
    
    public MealFood Update(MealFood mealFood)
    {
        _dbContext.MealFoods.Update(mealFood);
        return mealFood;
    }
}