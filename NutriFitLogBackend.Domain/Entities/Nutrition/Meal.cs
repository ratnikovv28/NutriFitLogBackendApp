using NutriFitLogBackend.Domain.Entities.Users;

namespace NutriFitLogBackend.Domain.Entities.Nutrition;

public class Meal
{
    public long Id { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    
    public long UserId { get; set; }
    public User? User { get; set; }
    
    public List<MealFood> Foods { get; set; } = new();
}