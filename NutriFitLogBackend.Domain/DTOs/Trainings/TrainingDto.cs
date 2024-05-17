namespace NutriFitLogBackend.Domain.DTOs.Trainings;

public class TrainingDto
{
    public long Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<TrainingExerciseDto> Exercises { get; set; }
}