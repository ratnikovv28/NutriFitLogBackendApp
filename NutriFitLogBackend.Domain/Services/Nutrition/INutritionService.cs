using NutriFitLogBackend.Domain.DTOs.Nutrition;
using NutriFitLogBackend.Domain.DTOs.Trainings;

namespace NutriFitLogBackend.Domain.Services.Nutrition;

public interface INutritionService
{
    Task<MealDto> CreateMeal(CreateMealDto createMealDto);
    Task<IEnumerable<MealDto>> GetAllMeals();
    Task<MealDto> UpdateMeal(UpdateMealDto updateMealDto);
    Task DeleteMeal(long id);
}