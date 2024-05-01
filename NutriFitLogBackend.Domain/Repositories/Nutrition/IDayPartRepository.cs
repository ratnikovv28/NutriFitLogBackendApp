using NutriFitLogBackend.Domain.Entities.Nutrition;

namespace NutriFitLogBackend.Domain.Repositories.Nutrition;

public interface IDayPartRepository
{
    Task<IReadOnlyCollection<DayPart>> GetAllAsync();
}