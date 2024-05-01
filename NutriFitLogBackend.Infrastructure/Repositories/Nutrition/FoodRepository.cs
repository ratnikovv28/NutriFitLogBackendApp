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
        
    public async Task<Food> GetByIdAsync(long id)
    {
        return await _dbContext.Foods.FindAsync(id);
    }
    
    public async Task<IReadOnlyCollection<Food>> GetAllAsync()
    {
        return await _dbContext.Foods.ToListAsync();
    }

    public async Task<Food> AddAsync(Food food)
    {
        await _dbContext.Foods.AddAsync(food);
        return food;
    }

    public async Task UpdateAsync(Food food)
    {
        await Task.Run(() => _dbContext.Foods.Update(food));
    }
 
    public async Task DeleteAsync(Food food)
    {
        await Task.Run(() => _dbContext.Foods.Remove(food));
    }
}