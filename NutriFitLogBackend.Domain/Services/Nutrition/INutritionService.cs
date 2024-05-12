using NutriFitLogBackend.Domain.DTOs.Nutrition;
using NutriFitLogBackend.Domain.DTOs.Trainings;

namespace NutriFitLogBackend.Domain.Services.Nutrition;

public interface INutritionService
{
    Task<IReadOnlyCollection<DayPartDto>> GetAllDayPartsAsync();
    Task<IReadOnlyCollection<FoodDto>> GetAllFoodsAsync();
    Task<IReadOnlyCollection<MealFoodDto>> GetUserFoodsByDateAsync(long telegramId, DateOnly date, long trainerId = 0);
    Task AddFoodAsync(RequestFoodDto dto);
    Task DeleteFoodAsync(long telegramId, long mealId, long foodId, long dayPartId, long trainerId = 0);
    Task UpdateFoodMealAsync(RequestFoodDto dto);
}