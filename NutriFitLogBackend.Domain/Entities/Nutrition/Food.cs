namespace NutriFitLogBackend.Domain.Entities.Nutrition;

public class Food
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = String.Empty;
    public double? Calories { get; set; }
    public double? Protein { get; set; }
    public double? Fats { get; set; }
    public double? Carbohydrates { get; set; }
    
    public List<MealFood> Meals { get; set; } = new();
}