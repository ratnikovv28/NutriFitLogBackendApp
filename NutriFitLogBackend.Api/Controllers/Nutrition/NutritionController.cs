using Microsoft.AspNetCore.Mvc;
using NutriFitLogBackend.Domain.DTOs.Nutrition;
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

    [HttpPost("Create")]
    public async Task<ActionResult<MealDto>> CreateMeal([FromBody] CreateMealDto dto)
    {
        var createdMeal = await _nutritionService.CreateMeal(dto);
        return Ok(createdMeal);
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<MealDto>>> GetAllMeals()
    {
        var meals = await _nutritionService.GetAllMeals();
        return Ok(meals);
    }

    [HttpPut("Update")]
    public async Task<ActionResult<MealDto>> UpdateMeal([FromBody] UpdateMealDto dto)
    {
        var updatedMeal = await _nutritionService.UpdateMeal(dto);
        return Ok(updatedMeal);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteMeal(long id)
    {
        await _nutritionService.DeleteMeal(id);
        return Ok();
    }
}