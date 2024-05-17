namespace NutriFitLogBackend.Domain.Entities.Users;

public class StudentTrainer
{
    public long Id { get; set; }
    public bool IsWorking { get; set; }
    
    public long StudentId { get; set; }
    public User? Student { get; set; }
    
    public long TrainerId { get; set; }
    public User? Trainer { get; set; }
}