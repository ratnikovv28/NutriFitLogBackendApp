using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Users;
using NutriFitLogBackend.Domain.Repositories.Users;
using NutriFitLogBackend.Infrastructure.Database;

namespace NutriFitLogBackend.Infrastructure.Repositories.Users;

public class UserRepository : IUserRepository
{
    private readonly NutriFitLogContext _dbContext;

    public UserRepository(NutriFitLogContext dbContext)
    {
        _dbContext = dbContext;
    }
        
    public async Task<User> GetByIdAsync(long id)
    {
        return await _dbContext.Users.FindAsync(id);
    }
    
    public async Task<User> GetJustByTelegramIdAsync(long telegramId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.TelegramId == telegramId);
    }
    
    public async Task<User> GetByTelegramIdAsync(long telegramId)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.Trainings)
                .ThenInclude(t => t.Exercises)
                    .ThenInclude(t => t.Exercise)
            .Include(u => u.Trainings)
                .ThenInclude(t => t.Exercises)
                    .ThenInclude(te => te.Sets)
            .Include(u => u.Meals)
                .ThenInclude(m => m.Foods)
                    .ThenInclude(mf => mf.Food)
            .Include(u => u.Meals)
                .ThenInclude(f => f.Foods)
                    .ThenInclude(mf => mf.DayPart)
            .Include(u => u.Students)
            .Include(u => u.Trainers)
            .FirstOrDefaultAsync(u => u.TelegramId == telegramId);
    }

    public async Task<IReadOnlyCollection<User>> GetAllAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User> AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        return user;
    }

    public void UpdateAsync(User user)
    {
        _dbContext.Users.Update(user);
    }
 
    public void DeleteAsync(User user)
    {
        _dbContext.Users.Remove(user);
    }
    
    public Task<bool> ExistAsync(long telegramId)
    {
        return _dbContext.Users.AnyAsync(u => u.TelegramId == telegramId);
    }
}