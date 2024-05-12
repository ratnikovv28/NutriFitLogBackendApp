namespace NutriFitLogBackend.Domain.Entities.Trainings;

public class Set
{
    public long Id { get; set; }
    
    public long? Repetitions { get; set; }
    public double? Weight { get; set; }
    public double? Duration { get; set; } 
    public double? Distance { get; set; }
    
    public long TrainingId { get; set; }
    public long ExerciseId { get; set; }

    public TrainingExercise TrainingExercise { get; set; }
}