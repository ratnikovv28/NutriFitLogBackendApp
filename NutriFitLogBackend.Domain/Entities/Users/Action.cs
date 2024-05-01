namespace NutriFitLogBackend.Domain.Entities.Users;

public class Action
{
    public long Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Description { get; set; } = String.Empty;
    
    public long AdminId { get; set; }
    public User? Admin { get; set; }

    public Action() {}
    
    public Action(string description, User admin)
    {
        Description = description;
        Admin = admin;
        CreatedDate = DateTime.UtcNow;
    }
}

