using System.Diagnostics.CodeAnalysis;
using NutriFitLogBackend.Domain.Entities.Users;

namespace NutriFitLogBackend.Domain.Entities.Trainings;

public class Training
{
    public long Id { get; set; }
    public DateTime CreatedDate { get; set; }
    
    public long UserId { get; [ExcludeFromCodeCoverage] set; }
    public User? User { get; set; }
    
    public List<TrainingExercise> Exercises { get; set; } = new();
}