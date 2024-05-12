namespace NutriFitLogBackend.Domain.DTOs.Nutrition.RequestDTOs;

public class UserFoodsByDateDto
{
    public long TelegramId { get; set; }
    public DateOnly Date { get; set; }
    public long TrainerId { get; set; } = -1;
}