using NutriFitLogBackend.Domain.Entities.Nutrition;

namespace NutriFitLogBackend.Domain.Repositories.Nutrition;

public interface IFoodRepository
{
    Task<Food> GetByIdAsync(long id);
    Task<IReadOnlyCollection<Food>> GetAllAsync();
    Task<Food> AddAsync(Food food);
    Task UpdateAsync(Food food);
    Task DeleteAsync(Food food);
}