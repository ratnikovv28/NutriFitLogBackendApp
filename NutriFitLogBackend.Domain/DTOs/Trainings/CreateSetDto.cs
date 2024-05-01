namespace NutriFitLogBackend.Domain.DTOs.Trainings;

public class CreateSetDto
{
    public long? Repetitions { get; set; }  
    public double? Weight { get; set; }     
    public TimeSpan? Duration { get; set; } 
    public double? Distance { get; set; }   
}