namespace NutriFitLogBackend.Domain.DTOs.Nutrition;

public class UpdateMealDto
{
    public long Id { get; set; } 
    public List<UpdateMealFoodDto> Foods { get; set; } 
}