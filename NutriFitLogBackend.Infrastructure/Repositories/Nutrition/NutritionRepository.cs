using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Nutrition;
using NutriFitLogBackend.Domain.Repositories.Nutrition;
using NutriFitLogBackend.Infrastructure.Database;

namespace NutriFitLogBackend.Infrastructure.Repositories.Nutrition;

public class NutritionRepository : INutritionRepository
{
    private readonly NutriFitLogContext _dbContext;

    public NutritionRepository(NutriFitLogContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Meal> GetByIdAsync(long id)
    {
        return await _dbContext.Meals
            .Include(m => m.Foods)
            .ThenInclude(mf => mf.Food)
            .FirstOrDefaultAsync(m => m.Id == id);
    }
    
    public async Task<IReadOnlyCollection<Meal>> GetAllByTelegramIdAsync(long telegramId)
    {
        return await _dbContext.Meals
            .Where(m => m.User.TelegramId == telegramId)
            .Include(m => m.Foods)
            .ThenInclude(mf => mf.Food)
            .ToListAsync();
    }
    
    public async Task<IReadOnlyCollection<Meal>> GetAllAsync()
    {
        return await _dbContext.Meals
            .Include(m => m.Foods)
            .ThenInclude(mf => mf.Food)
            .ToListAsync();
    }

    public async Task<Meal> AddAsync(Meal meal)
    {
        await _dbContext.Meals.AddAsync(meal);
        await _dbContext.SaveChangesAsync();
        return meal;
    }

    public async Task UpdateAsync(Meal meal)
    {
        _dbContext.Meals.Update(meal);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Meal meal)
    {
        _dbContext.Meals.Remove(meal);
        await _dbContext.SaveChangesAsync();
    }
}