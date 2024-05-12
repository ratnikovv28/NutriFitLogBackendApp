using NutriFitLogBackend.Domain.DTOs.Trainings;
using NutriFitLogBackend.Domain.Entities.Trainings;

namespace NutriFitLogBackend.Domain.Services.Trainings;

public interface ITrainingService
{
    Task<IReadOnlyCollection<ExerciseDto>> GetAllExercisesAsync();

    Task<TrainingDto> GetUserExercisesByDateAsync(long telegramId, DateOnly date,
        long trainerId = -1);

    Task AddExerciseAsync(long telegramId, long trainingId, long exerciseId, long trainerId = -1);

    Task UpdateSetsExerciseAsync(long telegramId, long trainingId, long exerciseId, IReadOnlyCollection<SetDto> setsDto,
        long trainerId = -1);
    
    Task DeleteSetsExerciseAsync(long telegramId, long trainingId, long exerciseId, long setId,
        long trainerId = -1);
    
    Task DeleteExerciseAsync(long telegramId, long trainingId, long exerciseId, long trainerId = -1);
}