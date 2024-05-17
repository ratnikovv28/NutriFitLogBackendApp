using NutriFitLogBackend.Domain.DTOs.Trainings;
using NutriFitLogBackend.Domain.Entities.Trainings;

namespace NutriFitLogBackend.Domain.Services.Trainings;

public interface ITrainingService
{
    Task<IReadOnlyCollection<ExerciseDto>> GetAllExercisesAsync();

    Task<TrainingDto> GetUserExercisesByDateAsync(long telegramId, DateOnly date,
        long trainerId = 0);

    Task<IReadOnlyCollection<ExerciseDto>> GetAvailableUserExercisesAsync(long telegramId, long trainingId,
        long trainerId = 0);

    Task AddExerciseAsync(long telegramId, long trainingId, long exerciseId, long trainerId = 0);

    Task UpdateSetsExerciseAsync(long telegramId, long trainingId, long exerciseId, IReadOnlyCollection<SetDto> setsDto,
        long trainerId = 0);

    Task DeleteSetsExerciseAsync(long telegramId, long trainingId, long exerciseId, IReadOnlyCollection<long> setsId,
        long trainerId = 0);
    
    Task DeleteExerciseAsync(long telegramId, long trainingId, long exerciseId, long trainerId = 0);

    Task<IReadOnlyCollection<SetDto>> GetExerciseSetsAsync(long trainingId, long exerciseId);
}