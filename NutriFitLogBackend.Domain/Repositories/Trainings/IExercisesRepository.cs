using NutriFitLogBackend.Domain.Entities.Trainings;

namespace NutriFitLogBackend.Domain.Repositories.Trainings;

public interface IExercisesRepository
{
    Task<Exercise> GetByIdAsync(long id);
    Task<IReadOnlyCollection<Exercise>> GetAllAsync();
    Task<Exercise> AddAsync(Exercise exercise);
    Task UpdateAsync(Exercise exercise);
    void DeleteAsync(Exercise exercise);
    Task<bool> ExistAsync(long exerciseId);
}