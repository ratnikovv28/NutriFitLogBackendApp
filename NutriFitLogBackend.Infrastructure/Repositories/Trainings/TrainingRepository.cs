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

    public async Task<Training> GetByIdAsync(long id)
    {
        return await _dbContext.Trainings
            .Include(t => t.Exercises)
            .ThenInclude(e => e.Sets)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
    
    public async Task<IReadOnlyCollection<Training>> GetAllByTelegramIdAsync(long telegramId)
    {
        return await _dbContext.Trainings
            .Where(t => t.User.TelegramId == telegramId)
            .Include(t => t.Exercises)
            .ThenInclude(e => e.Sets)
            .ToListAsync();
    }
    
    public async Task<IReadOnlyCollection<Training>> GetAllAsync()
    {
        return await _dbContext.Trainings
            .Include(t => t.Exercises)
            .ThenInclude(e => e.Sets)
            .ToListAsync();
    }

    public async Task<Training> AddAsync(Training training)
    {
        await _dbContext.Trainings.AddAsync(training);
        await _dbContext.SaveChangesAsync();
        return training;
    }

    public async Task UpdateAsync(Training training)
    {
        _dbContext.Trainings.Update(training);
        await _dbContext.SaveChangesAsync();
    }
 
    public async Task DeleteAsync(Training training)
    {
        _dbContext.Trainings.Remove(training);
        await _dbContext.SaveChangesAsync();
    }
}