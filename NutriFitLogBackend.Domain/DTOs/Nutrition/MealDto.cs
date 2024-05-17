namespace NutriFitLogBackend.Domain.DTOs.Nutrition;

public class MealDto
{
    public long Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<MealFoodDto> Foods { get; set; }
}