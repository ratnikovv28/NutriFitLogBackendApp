using NutriFitLogBackend.Domain.Entities.Trainings;

namespace NutriFitLogBackend.Domain.Repositories.Trainings;

public interface ITrainingExerciseRepository
{
    Task<TrainingExercise> AddAsync(TrainingExercise trainingExercise);
    void Delete(TrainingExercise trainingExercise);
    Task<TrainingExercise> GetByTrainingAndExercideId(long trainingId, long exerciseId);
}