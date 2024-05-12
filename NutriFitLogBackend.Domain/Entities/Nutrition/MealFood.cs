namespace NutriFitLogBackend.Domain.Entities.Nutrition;

public class MealFood
{
    public long Id { get; set; }
    public double? Calories { get; set; }
    public double? Protein { get; set; }
    public double? Fats { get; set; }
    public double? Carbohydrates { get; set; }    
    
    public UnitOfMeasure Unit { get; set; }
    public double Quantity { get; set; }
    
    public long DayPartId { get; set; }
    public DayPart? DayPart { get; set; }
    
    public long MealId { get; set; }
    public Meal? Meal { get; set; }
    
    public long FoodId { get; set; }
    public Food? Food { get; set; }
}