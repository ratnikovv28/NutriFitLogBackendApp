using System.Diagnostics.CodeAnalysis;

namespace NutriFitLogBackend.Domain.Entities.Nutrition;

public class Food
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = String.Empty;
    public string PictureUrl { get; set; } = String.Empty;
    public UnitOfMeasure Unit { get; set; }
    public List<MealFood> Meals { get; [ExcludeFromCodeCoverage] set; } = new();
}