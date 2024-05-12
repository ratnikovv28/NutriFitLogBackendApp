using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Trainings;
using NutriFitLogBackend.Domain.Repositories.Trainings;
using NutriFitLogBackend.Infrastructure.Database;

namespace NutriFitLogBackend.Infrastructure.Repositories.Trainings;

public class SetRepository : ISetRepository
{
    private readonly NutriFitLogContext _dbContext;

    public SetRepository(NutriFitLogContext dbContext)
    {
        _dbContext = dbContext;
    }
        
    public async Task<Set> GetByIdAsync(long id)
    {
        return await _dbContext.Sets.FindAsync(id);
    }
    
    public async Task<IReadOnlyCollection<Set>> GetAllAsync()
    {
        return await _dbContext.Sets.ToListAsync();
    }

    public async Task<Set> AddAsync(Set set)
    {
        await _dbContext.Sets.AddAsync(set);
        return set;
    }

    public void Update(Set set)
    {
        _dbContext.Sets.Update(set);
    }
 
    public void Delete(Set set)
    {
        _dbContext.Sets.Remove(set);
    }
    
    public void DeleteRangeAsync(IReadOnlyCollection<Set> sets)
    {
        _dbContext.Sets.RemoveRange(sets);
    }
}