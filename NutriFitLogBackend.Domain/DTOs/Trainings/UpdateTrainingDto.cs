namespace NutriFitLogBackend.Domain.DTOs.Trainings;

public class UpdateTrainingDto
{
    public long Id { get; set; } 
    public List<UpdateTrainingExerciseDto> Exercises { get; set; }
}