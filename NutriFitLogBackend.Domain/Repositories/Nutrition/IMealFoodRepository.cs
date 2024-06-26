using NutriFitLogBackend.Domain.Entities.Nutrition;

namespace NutriFitLogBackend.Domain.Repositories.Nutrition;

public interface IMealFoodRepository
{
    Task<MealFood> GetById(long mealFoodId);
    Task<MealFood> AddAsync(MealFood mealFood);
    void Delete(MealFood mealFood);
    MealFood Update(MealFood mealFood);
}