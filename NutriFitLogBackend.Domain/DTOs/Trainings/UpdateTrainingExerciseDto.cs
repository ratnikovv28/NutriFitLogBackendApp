namespace NutriFitLogBackend.Domain.DTOs.Trainings;

public class UpdateTrainingExerciseDto
{
    public long Id { get; set; } 
    public List<UpdateSetDto> Sets { get; set; }
}