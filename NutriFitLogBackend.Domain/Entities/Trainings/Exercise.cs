namespace NutriFitLogBackend.Domain.Entities.Trainings;

public class Exercise
{
    public long Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public string PictureUrl { get; set; } = String.Empty;
    public DateTime CreatedDate { get; set; }
    public ExerciseType Type { get; set; }
    
    public List<TrainingExercise> Trainings { get; set; } = new();

    public Exercise(
        string name,
        string description,
        string pictureUrl,
        ExerciseType type)
    {
        CreatedDate = DateTime.UtcNow;
        Name = name;
        Description = description;
        PictureUrl = pictureUrl;
        Type = type;
    }
}