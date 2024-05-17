using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Trainings;
using NutriFitLogBackend.Domain.Repositories.Trainings;
using NutriFitLogBackend.Infrastructure.Database;

namespace NutriFitLogBackend.Infrastructure.Repositories.Trainings;

public class TrainingExerciseRepository : ITrainingExerciseRepository
{
    private readonly NutriFitLogContext _dbContext;

    public TrainingExerciseRepository(NutriFitLogContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<TrainingExercise> AddAsync(TrainingExercise trainingExercise)
    {
        await _dbContext.TrainingExercise.AddAsync(trainingExercise);
        return trainingExercise;
    }
    
    public void Delete(TrainingExercise trainingExercise)
    {
        _dbContext.Entry(trainingExercise).State = EntityState.Detached;
        _dbContext.TrainingExercise.Remove(trainingExercise);
    }
    
    public async Task<TrainingExercise> GetByTrainingAndExercideId(long trainingId, long exerciseId)
    {
        var trainingExercises = await _dbContext.TrainingExercise.FindAsync(trainingId, exerciseId);
        return trainingExercises;
    }
}