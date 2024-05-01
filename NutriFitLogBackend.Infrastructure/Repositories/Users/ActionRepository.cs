using NutriFitLogBackend.Domain.Repositories.Users;
using NutriFitLogBackend.Infrastructure.Database;
using Action = NutriFitLogBackend.Domain.Entities.Users.Action;

namespace NutriFitLogBackend.Infrastructure.Repositories.Users;

public class ActionRepository : IActionRepository
{
    private readonly NutriFitLogContext _dbContext;

    public ActionRepository(NutriFitLogContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Action> GetByIdAsync(long id)
    {
        return await _dbContext.Actions.FindAsync(id);
    }
    
    public async Task<Action> AddAsync(Action action)
    {
        await _dbContext.Actions.AddAsync(action);
        return action;
    }
}