using NutriFitLogBackend.Domain.Entities.Users;

namespace NutriFitLogBackend.Domain.DTOs.Users;

public class UserDto
{
    public long TelegramId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActiveTrainer { get; set; }
    public List<string> Roles { get; set; } = new();
}