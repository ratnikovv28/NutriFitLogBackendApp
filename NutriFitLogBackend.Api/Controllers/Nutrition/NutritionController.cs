using Microsoft.AspNetCore.Mvc;
using NutriFitLogBackend.Domain.DTOs.Nutrition;
using NutriFitLogBackend.Domain.DTOs.Nutrition.RequestDTOs;
using NutriFitLogBackend.Domain.Services.Nutrition;

namespace NutriFitLogBackend.Controllers.Nutrition;

[ApiController]
[Route("[controller]")]
public class NutritionController : ControllerBase
{
    private readonly INutritionService _nutritionService;

    public NutritionController(INutritionService nutritionService)
    {
        _nutritionService = nutritionService ?? throw new ArgumentNullException(nameof(nutritionService));
    }

    [HttpGet("GetAllDayParts")]
    public async Task<ActionResult<IReadOnlyCollection<DayPartDto>>> GetAllDayParts()
    {
        return Ok(await _nutritionService.GetAllDayPartsAsync());
    }
    
    [HttpPost("GetAvailableUserFoods")]
    public async Task<ActionResult<IReadOnlyCollection<FoodDto>>> GetAvailableUserFoods([FromBody] AvailableUserFoodDto dto)
    {
        return Ok(await _nutritionService.GetAvailableUserFoodAsync(dto.TelegramId, dto.MealId, dto.DayPartId, dto.TrainerId));
    }
    
    [HttpGet("GetAllFoods")]
    public async Task<ActionResult<IReadOnlyCollection<FoodDto>>> GetAllFoods()
    {
        return Ok(await _nutritionService.GetAllFoodsAsync());
    }
    
    [HttpPost("GetUserMealByDate")]
    public async Task<ActionResult<MealDto>> GetUserMealByDate([FromBody] UserFoodsByDateDto dto)
    {
        return Ok(await _nutritionService.GetUserMealByDateAsync(dto.TelegramId, dto.Date, dto.TrainerId));
    }
    
    [HttpPost("AddUserFood")]
    public async Task<ActionResult> AddUserFood([FromBody] RequestFoodDto dto)
    {
        await _nutritionService.AddFoodAsync(dto);
        return Ok();
    }

    [HttpPut("UpdateFoodMeal")]
    public async Task<ActionResult> UpdateFoodMeal([FromBody] RequestFoodDto dto)
    {
        await _nutritionService.UpdateFoodMealAsync(dto);
        return Ok();
    }
    
    [HttpDelete("DeleteUserFood")]
    public async Task<ActionResult> DeleteFood([FromBody] DeleteUserFoodDto dto)
    {
        await _nutritionService.DeleteFoodAsync(dto.TelegramId, dto.MealId, dto.FoodId, dto.DayPartId, dto.TrainerId);
        return Ok();
    }
}