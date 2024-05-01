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
    
    public async Task<User> GetByTelegramIdAsync(long telegramId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.TelegramId == telegramId);
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

    public async Task UpdateAsync(User user)
    {
        await Task.Run(() => _dbContext.Users.Update(user));
    }
 
    public async Task DeleteAsync(User user)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public Task<bool> ExistAsync(long telegramId)
    {
        return _dbContext.Users.AnyAsync(u => u.TelegramId == telegramId);
    }
}