namespace NutriFitLogBackend.Domain.DTOs.Trainings;

public class CreateTrainingDto
{
    public DateTime CreatedDate { get; set; }
    public long UserId { get; set; }
    public List<CreateTrainingExerciseDto> Exercises { get; set; }
}