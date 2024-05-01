namespace NutriFitLogBackend.Domain.DTOs.Nutrition;

public class FoodDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double? Calories { get; set; }
    public double? Protein { get; set; }
    public double? Fats { get; set; }
    public double? Carbohydrates { get; set; }
}