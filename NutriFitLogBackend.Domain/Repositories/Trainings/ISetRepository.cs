using NutriFitLogBackend.Domain.Entities.Trainings;

namespace NutriFitLogBackend.Domain.Repositories.Trainings;

public interface ISetRepository
{
    Task<Set> AddAsync(Set set);
    void Update(Set set);
    void Delete(Set set);
    Task<IReadOnlyCollection<Set>> GetByTrainingAndExerciseIdAsync(long trainingId, long exerciseId);
    Task<Set> GetByTrainingAndExerciseIdAndIdAsync(long trainingId, long exerciseId, long setId);
}