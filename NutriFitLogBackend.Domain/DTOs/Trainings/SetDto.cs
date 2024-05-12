namespace NutriFitLogBackend.Domain.DTOs.Trainings;

public class SetDto
{
    public long Id { get; set; }
    public long? Repetitions { get; set; }
    public double? Weight { get; set; }
    public double? Duration { get; set; }
    public double? Distance { get; set; }
}