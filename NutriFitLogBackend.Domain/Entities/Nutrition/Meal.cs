using System.Diagnostics.CodeAnalysis;
using NutriFitLogBackend.Domain.Entities.Users;

namespace NutriFitLogBackend.Domain.Entities.Nutrition;

public class Meal
{
    public long Id { get; set; }
    public DateTime CreatedDate { get; set; }
    
    public long UserId { get; set; }
    public User? User { get; [ExcludeFromCodeCoverage] set; }
    
    public List<MealFood> Foods { get; set; } = new();
}