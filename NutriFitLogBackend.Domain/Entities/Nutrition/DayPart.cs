namespace NutriFitLogBackend.Domain.Entities.Nutrition;

public class DayPart
{
    public long Id { get; set; }
    public string Name { get; set; } = String.Empty;

    public List<MealFood> Meals { get; set; } = new();
}