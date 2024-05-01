namespace NutriFitLogBackend.Domain.Entities.Trainings;

public class Set
{
    public long Id { get; set; }
    
    public long? Repetitions { get; set; }
    public double? Weight { get; set; }
    public TimeSpan? Duration { get; set; } 
    public double? Distance { get; set; }
    
    public long TrainingExerciseId { get; set; }
    public TrainingExercise? TrainingExercise { get; set; }
}