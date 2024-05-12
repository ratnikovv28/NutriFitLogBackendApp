using NutriFitLogBackend.Domain.Entities.Nutrition;

namespace NutriFitLogBackend.Domain.Repositories.Nutrition;

public interface INutritionRepository
{
    Task<Meal> AddAsync(Meal meal);
}