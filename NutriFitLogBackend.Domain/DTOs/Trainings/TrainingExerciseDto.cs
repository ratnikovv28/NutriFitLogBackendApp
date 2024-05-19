using System.Diagnostics.CodeAnalysis;

namespace NutriFitLogBackend.Domain.DTOs.Trainings;

public class TrainingExerciseDto
{
    public long Id { [ExcludeFromCodeCoverage] get; set; }
    public ExerciseDto Exercise { [ExcludeFromCodeCoverage] get; set; }
    public List<SetDto> Sets { [ExcludeFromCodeCoverage] get; set; }
}