using NutriFitLogBackend.Domain.Entities.Nutrition;

namespace NutriFitLogBackend.Domain.DTOs.Nutrition;

public class MealFoodDto
{
    public long Id { get; set; }
    public string PortionDescription { get; set; }
    public UnitOfMeasure Unit { get; set; }
    public double Quantity { get; set; }
    public FoodDto Food { get; set; }
    public DayPartDto DayPart { get; set; }
}