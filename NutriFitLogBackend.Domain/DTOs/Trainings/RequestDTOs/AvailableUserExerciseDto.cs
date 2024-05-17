namespace NutriFitLogBackend.Domain.DTOs.Trainings.RequestDTOs;

public class AvailableUserExerciseDto
{
    public long TelegramId { get; set; }
    public long TrainingId { get; set; }
    public long TrainerId { get; set; } = 0;
}