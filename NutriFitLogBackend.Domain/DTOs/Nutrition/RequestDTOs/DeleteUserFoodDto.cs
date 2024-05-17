namespace NutriFitLogBackend.Domain.DTOs.Nutrition.RequestDTOs;

public class DeleteUserFoodDto
{
    public long TelegramId { get; set; }
    public long MealId { get; set; }
    public long FoodId { get; set; }
    public long DayPartId { get; set; }
    public long TrainerId { get; set; } = 0;
}