namespace NutriFitLogBackend.Domain.DTOs.Trainings.RequestDTOs;

public class DeleteSetsExerciseDto
{
    public long TelegramId { get; set; }
    public long TrainingId { get; set; }
    public long ExerciseId { get; set; }
    public long SetId { get; set; }
    public long TrainerId { get; set; } = -1;
}