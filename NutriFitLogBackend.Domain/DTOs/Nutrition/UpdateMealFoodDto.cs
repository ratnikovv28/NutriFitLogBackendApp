using NutriFitLogBackend.Domain.Entities.Nutrition;

namespace NutriFitLogBackend.Domain.DTOs.Nutrition;

public class UpdateMealFoodDto
{
    public long Id { get; set; }
    public string PortionDescription { get; set; }
    public UnitOfMeasure Unit { get; set; }
    public double Quantity { get; set; }
    public long FoodId { get; set; }
}