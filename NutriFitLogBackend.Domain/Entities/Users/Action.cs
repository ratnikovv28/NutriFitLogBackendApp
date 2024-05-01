namespace NutriFitLogBackend.Domain.Entities.Users;

public class Action
{
    public long Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Description { get; set; } = String.Empty;
    
    public long AdminId { get; set; }
    public User? Admin { get; set; }
}