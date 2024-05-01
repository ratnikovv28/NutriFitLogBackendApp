using Action = NutriFitLogBackend.Domain.Entities.Users.Action;

namespace NutriFitLogBackend.Domain.Repositories.Users;

public interface IActionRepository
{
    Task<Action> AddAsync(Action action);
    Task<Action> GetByIdAsync(long id);
}