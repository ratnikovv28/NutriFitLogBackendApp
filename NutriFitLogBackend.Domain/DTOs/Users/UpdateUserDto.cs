using NutriFitLogBackend.Domain.Entities.Users;

namespace NutriFitLogBackend.Domain.DTOs.Users;

public class UpdateUserDto
{
    public long TelegramId { get; set; }
    public List<UserRole> Roles { get; set; } = new();
}