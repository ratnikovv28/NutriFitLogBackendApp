using NutriFitLogBackend.Domain.Entities.Trainings;

namespace NutriFitLogBackend.Domain.Repositories.Trainings;

public interface ISetRepository
{
    Task<Set> GetByIdAsync(long id);
    Task<IReadOnlyCollection<Set>> GetAllAsync();
    Task<Set> AddAsync(Set set);
    void Update(Set set);
    void Delete(Set set);
    void DeleteRangeAsync(IReadOnlyCollection<Set> sets);
}