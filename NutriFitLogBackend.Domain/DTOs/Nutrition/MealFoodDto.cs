using System.Diagnostics.CodeAnalysis;

namespace NutriFitLogBackend.Domain.DTOs.Nutrition;

public class MealFoodDto
{
    public long Id { [ExcludeFromCodeCoverage] get; set; }
    public double Calories { [ExcludeFromCodeCoverage] get; set; }
    public double Protein { [ExcludeFromCodeCoverage] get; set; }
    public double Fats { [ExcludeFromCodeCoverage] get; set; }
    public double Carbohydrates { [ExcludeFromCodeCoverage] get; set; }    
    public double Quantity { [ExcludeFromCodeCoverage] get; set; }

    public FoodDto Food { [ExcludeFromCodeCoverage] get; set; }
    public DayPartDto DayPart { [ExcludeFromCodeCoverage] get; set; }
}