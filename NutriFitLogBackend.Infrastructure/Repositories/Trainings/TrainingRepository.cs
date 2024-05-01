using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Trainings;
using NutriFitLogBackend.Domain.Repositories.Trainings;
using NutriFitLogBackend.Infrastructure.Database;

namespace NutriFitLogBackend.Infrastructure.Repositories.Trainings;

public class TrainingRepository : ITrainingRepository
{
    private readonly NutriFitLogContext _dbContext;

    public TrainingRepository(NutriFitLogContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Training> GetTrainingByIdAsync(long id)
    {
        return await _dbContext.Trainings
            .Include(t => t.Exercises)
            .ThenInclude(te => te.Exercise)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Training>> GetAllTrainingsAsync()
    {
        return await _dbContext.Trainings
            .Include(t => t.Exercises)
            .ThenInclude(te => te.Exercise)
            .ToListAsync();
    }

    public async Task<Training> AddTrainingAsync(Training training)
    {
        _dbContext.Trainings.Add(training);
        await _dbContext.SaveChangesAsync();
        return training;
    }

    public async Task UpdateTrainingAsync(Training training)
    {
        _dbContext.Trainings.Update(training);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteTrainingAsync(Training training)
    {
        _dbContext.Trainings.Remove(training);
        await _dbContext.SaveChangesAsync();
    }
}