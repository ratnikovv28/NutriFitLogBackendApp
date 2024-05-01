using NutriFitLogBackend.Domain.Entities.Nutrition;

namespace NutriFitLogBackend.Domain.Repositories.Nutrition;

public interface INutritionRepository
{
    Task<Meal> GetMealByIdAsync(long id);
    Task<IEnumerable<Meal>> GetAllMealsAsync();
    Task<Meal> AddMealAsync(Meal meal);
    Task UpdateMealAsync(Meal meal);
    Task DeleteMealAsync(Meal meal);
}