using NutriFitLogBackend.Domain.Entities.Nutrition;
using NutriFitLogBackend.Domain.Entities.Trainings;

namespace NutriFitLogBackend.Domain.Entities.Users;

public class User
{
    public long Id { get; set; }
    public string TelegramId { get; set; } = String.Empty;
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    
    public List<Role> Roles { get; set; } = new();
    public List<User> Trainers { get; set; } = new();
    public List<User> Students { get; set; } = new();
    public List<Action> Actions { get; set; } = new();
    public List<Training> Trainings { get; set; } = new();
    
    public List<Meal> Meals { get; set; } = new();
}