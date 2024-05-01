namespace NutriFitLogBackend.Domain.DTOs.Trainings;

public class TrainingExerciseDto
{
    public long Id { get; set; }
    public ExerciseDto Exercise { get; set; }
    public List<SetDto> Sets { get; set; }
}