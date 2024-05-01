using NutriFitLogBackend.Domain.DTOs.Users;

namespace NutriFitLogBackend.Domain.DTOs.Trainings;

public class TrainingDto
{
    public long Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public UserDto User { get; set; }
    public List<TrainingExerciseDto> Exercises { get; set; }
}