using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Nutrition;
using NutriFitLogBackend.Domain.Repositories.Nutrition;
using NutriFitLogBackend.Infrastructure.Database;

namespace NutriFitLogBackend.Infrastructure.Repositories.Nutrition;

public class FoodRepository : IFoodRepository
{
    private readonly NutriFitLogContext _dbContext;

    public FoodRepository(NutriFitLogContext dbContext)
    {
        _dbContext = dbContext;
    }
        
    public async Task<IReadOnlyCollection<Food>> GetAllAsync()
    {
        return await _dbContext.Foods.ToListAsync();
    }

    public Task<bool> ExistAsync(long foodId)
    {
        return _dbContext.Foods.AnyAsync(e => e.Id == foodId);
    }
}