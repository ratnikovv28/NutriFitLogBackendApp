using NutriFitLogBackend.Domain.Entities.Nutrition;

namespace NutriFitLogBackend.Domain.Repositories.Nutrition;

public interface INutritionRepository
{
    Task<Meal> GetByIdAsync(long id);
    Task<IReadOnlyCollection<Meal>> GetAllByTelegramIdAsync(long telegramId);
    Task<IReadOnlyCollection<Meal>> GetAllAsync();
    Task<Meal> AddAsync(Meal meal);
    Task UpdateAsync(Meal meal);
    Task DeleteAsync(Meal meal);
}