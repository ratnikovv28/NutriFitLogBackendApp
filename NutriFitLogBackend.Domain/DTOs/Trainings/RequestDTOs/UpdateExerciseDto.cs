namespace NutriFitLogBackend.Domain.DTOs.Trainings.RequestDTOs;

public class UpdateExerciseDto
{
    public long TelegramId { get; set; }
    public long TrainingId { get; set; }
    public long ExerciseId { get; set; }
    public List<SetDto> Sets { get; set; }
    public long TrainerId { get; set; } = -1;
}