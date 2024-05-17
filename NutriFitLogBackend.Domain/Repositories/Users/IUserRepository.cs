using NutriFitLogBackend.Domain.Entities.Users;

namespace NutriFitLogBackend.Domain.Repositories.Users;

public interface IUserRepository
{
    Task<User> GetByTelegramIdAsync(long id);
    Task<User> GetJustByTelegramIdAsync(long telegramId);
    Task<User> GetByIdAsync(long id);
    Task<IReadOnlyCollection<User>> GetAllAsync();
    Task<User> AddAsync(User user);
    void UpdateAsync(User user);
    void DeleteAsync(User user);
    Task<bool> ExistAsync(long telegramId);
}