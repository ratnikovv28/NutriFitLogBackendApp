namespace NutriFitLogBackend.Domain.Entities.Trainings;

public class TrainingExercise
{
    public long Id { get; set; }
    
    public long TrainingId { get; set; }
    public Training? Training { get; set; }
    
    public long ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }

    public List<Set> Sets { get; set; } = new();
}