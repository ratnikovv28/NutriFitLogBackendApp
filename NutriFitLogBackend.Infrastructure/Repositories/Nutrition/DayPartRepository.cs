using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Nutrition;
using NutriFitLogBackend.Domain.Repositories.Nutrition;
using NutriFitLogBackend.Infrastructure.Database;

namespace NutriFitLogBackend.Infrastructure.Repositories.Nutrition;

public class DayPartRepository : IDayPartRepository
{
    private readonly NutriFitLogContext _dbContext;

    public DayPartRepository(NutriFitLogContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IReadOnlyCollection<DayPart>> GetAllAsync()
    {
        return await _dbContext.DayParts.ToListAsync();
    }
}