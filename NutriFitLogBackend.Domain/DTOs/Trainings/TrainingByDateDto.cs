namespace NutriFitLogBackend.Domain.DTOs.Trainings;

public class TrainingByDateDto
{
    public long TelegramId { get; set; }
    public DateOnly Date { get; set; }
}