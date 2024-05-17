using NutriFitLogBackend.Domain.Entities.Nutrition;

namespace NutriFitLogBackend.Domain.Entities.Users;

public class User
{
    public long Id { get; set; }
    public long TelegramId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActiveTrainer { get; set; }
    
    public List<UserRole> Roles { get; set; }
    public List<Trainings.Training> Trainings { get; set; } = new();
    
    public List<Meal> Meals { get; set; } = new();

    public List<StudentTrainer> Students { get; set; } = new();
    public List<StudentTrainer> Trainers { get; set; } = new();
    
    public User(long telegramId)
    {
        TelegramId = telegramId;
        CreatedDate = DateTime.UtcNow;
        IsActiveTrainer = true;
        Roles = new List<UserRole>
        {
            UserRole.User
        };
    }
}