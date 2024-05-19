using NutriFitLogBackend.Domain.DTOs.Users;
using NutriFitLogBackend.Domain.DTOs.Users.RequestDTOs;

namespace NutriFitLogBackend.Domain.Services.Users;

public interface IUserService
{
    Task<UserDto> GetUserByTelegramId(long telegramId);
    Task<UserDto> CreateUser(CreateUserDto userDto);
    Task<IReadOnlyCollection<UserDto>> GetTrainers(long telegramId);
    Task<IReadOnlyCollection<UserDto>> GetStudents(long telegramId, bool activeStudents);
    Task<UserDto> UpdateTrainerStatus(long telegramId);
    Task DeleteStudentTrainerRelationShip(long studentId, long trainerId);
    Task AddStudentToTrainer(long studentId, long trainerId);
    Task CreateStudentTrainer(long studentId, long trainerId);
}