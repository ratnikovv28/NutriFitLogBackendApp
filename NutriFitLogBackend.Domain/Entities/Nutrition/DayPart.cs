using System.Diagnostics.CodeAnalysis;

namespace NutriFitLogBackend.Domain.Entities.Nutrition;

public class DayPart
{
    public long Id { get; [ExcludeFromCodeCoverage] set; }
    public string Name { get; set; } = String.Empty;

    [ExcludeFromCodeCoverage]
    public List<MealFood> Meals { get; set; } = new();
}