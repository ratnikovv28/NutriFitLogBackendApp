namespace NutriFitLogBackend.Domain.DTOs.Trainings.RequestDTOs;

public class DeleteUserExerciseDto
{
    public long TelegramId { get; set; }
    public long TrainingId { get; set; }
    public long ExerciseId { get; set; }
    public long TrainerId { get; set; } = 0;
}