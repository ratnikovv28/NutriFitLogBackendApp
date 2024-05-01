namespace NutriFitLogBackend.Domain.DTOs.Nutrition;

public class CreateMealDto
{
    public DateTime CreatedDate { get; set; }
    public long UserId { get; set; }
    public List<CreateMealFoodDto> Foods { get; set; }
}