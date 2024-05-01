using NutriFitLogBackend.Domain.DTOs.Users;

namespace NutriFitLogBackend.Domain.Services;

public interface IUserService
{
    Task<IReadOnlyCollection<UserDto>> GetUsers();
    Task<UserDto> GetUserByTelegramId(long telegramId);
    Task<UserDto> CreateUser(CreateUserDto userDto);
    Task<UserDto> UpdateUser(UpdateUserDto userDto);
    Task DeleteUserByTelegramId(long telegramId);
}