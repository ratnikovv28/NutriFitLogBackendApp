namespace NutriFitLogBackend.Domain.DTOs.Trainings;

public class RequestExerciseDto
{
    public long TelegramId { get; set; }
    public long ExerciseId { get; set; }
    public DateOnly Date { get; set; }
}