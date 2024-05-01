using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Trainings;
using NutriFitLogBackend.Domain.Repositories.Trainings;
using NutriFitLogBackend.Infrastructure.Database;

namespace NutriFitLogBackend.Infrastructure.Repositories.Trainings;

public class ExercisesRepository : IExercisesRepository
{
    private readonly NutriFitLogContext _dbContext;

    public ExercisesRepository(NutriFitLogContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Exercise> GetByIdAsync(long id)
    {
        return await _dbContext.Exercises.FindAsync(id);
    }
    
    public async Task<IReadOnlyCollection<Exercise>> GetAllAsync()
    {
        return await _dbContext.Exercises.ToListAsync();
    }

    public async Task<Exercise> AddAsync(Exercise exercise)
    {
        await _dbContext.Exercises.AddAsync(exercise);
        return exercise;
    }

    public async Task UpdateAsync(Exercise exercise)
    {
        await Task.Run(() => _dbContext.Exercises.Update(exercise));
    }
 
    public async Task DeleteAsync(Exercise exercise)
    {
        await Task.Run(() => _dbContext.Exercises.Remove(exercise));
    }
}