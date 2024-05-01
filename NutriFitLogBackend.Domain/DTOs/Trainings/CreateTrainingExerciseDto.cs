namespace NutriFitLogBackend.Domain.DTOs.Trainings;

public class CreateTrainingExerciseDto
{
    public long ExerciseId { get; set; }
    public List<CreateSetDto> Sets { get; set; }
}