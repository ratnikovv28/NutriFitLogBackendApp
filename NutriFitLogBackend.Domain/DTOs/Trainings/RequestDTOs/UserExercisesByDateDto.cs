namespace NutriFitLogBackend.Domain.DTOs.Trainings.RequestDTOs;

public class UserExercisesByDateDto
{
    public long TelegramId { get; set; }
    public DateOnly Date { get; set; }
    public long TrainerId { get; set; } = 0;
}