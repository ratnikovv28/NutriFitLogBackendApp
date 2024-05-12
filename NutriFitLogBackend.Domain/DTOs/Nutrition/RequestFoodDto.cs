namespace NutriFitLogBackend.Domain.DTOs.Nutrition;

public class RequestFoodDto
{
    public long TelegramId { get; set; }
    public long MealId { get; set; }
    public long FoodId { get; set; }
    public long DayPartId { get; set; }
    public long TrainerId { get; set; }
    public double? Calories { get; set; }
    public double? Protein { get; set; }
    public double? Fats { get; set; }
    public double? Carbohydrates { get; set; }
    public double Quantity { get; set; }
}