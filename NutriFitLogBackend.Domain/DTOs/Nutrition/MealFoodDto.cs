namespace NutriFitLogBackend.Domain.DTOs.Nutrition;

public class MealFoodDto
{
    public long Id { get; set; }
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Fats { get; set; }
    public double Carbohydrates { get; set; }    
    public double Quantity { get; set; }

    public FoodDto Food { get; set; }
    public DayPartDto DayPart { get; set; }
}