using NutriFitLogBackend.Domain.Entities.Users;

namespace NutriFitLogBackend.Domain.Repositories.Users;

public interface IUserRepository
{
    Task<User> GetByTelegramIdAsync(long id);
    Task<IReadOnlyCollection<User>> GetAllAsync();
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<bool> ExistAsync(long telegramId);
}