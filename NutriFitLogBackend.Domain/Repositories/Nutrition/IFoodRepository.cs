using NutriFitLogBackend.Domain.Entities.Nutrition;

namespace NutriFitLogBackend.Domain.Repositories.Nutrition;

public interface IFoodRepository
{
    Task<IReadOnlyCollection<Food>> GetAllAsync();
    Task<bool> ExistAsync(long foodId);
}