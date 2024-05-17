namespace NutriFitLogBackend.Domain.DTOs.Nutrition.RequestDTOs;

public class AvailableUserFoodDto
{
    public long TelegramId { get; set; }
    public long MealId { get; set; }
    public long DayPartId { get; set; }
    public long TrainerId { get; set; } = 0;
}