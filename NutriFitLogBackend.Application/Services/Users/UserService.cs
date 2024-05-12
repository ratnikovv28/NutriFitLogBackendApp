using AutoMapper;
using NutriFitLogBackend.Domain;
using NutriFitLogBackend.Domain.DTOs.Users;
using NutriFitLogBackend.Domain.Entities.Users;
using NutriFitLogBackend.Domain.Exceptions;
using NutriFitLogBackend.Domain.Services;
using NutriFitLogBackend.Domain.Services.Users;

namespace NutriFitLogBackend.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<UserDto> CreateUser(CreateUserDto createUserDto)
    {   
        var exists = await _unitOfWork.UserRepository.ExistAsync(createUserDto.TelegramId);
        if (exists)
            throw new ConflictException($"The user with the ID = {createUserDto.TelegramId} already exists.");

        var user = new User(createUserDto.TelegramId);
        
        user = await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveAsync();
        
        return _mapper.Map<UserDto>(user);
    }
    
    public async Task<IReadOnlyCollection<UserDto>> GetUsers()
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync();
        return _mapper.Map<IReadOnlyCollection<UserDto>>(users);
    }

    public async Task<UserDto> GetUserByTelegramId(long telegramId)
    {
        var user = await _unitOfWork.UserRepository.GetByTelegramIdAsync(telegramId);
        if (user is null)
            throw new UserNotFoundException(telegramId);
        
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateUser(UpdateUserDto userDto)
    {
        var user = await _unitOfWork.UserRepository.GetByTelegramIdAsync(userDto.TelegramId);
        if (user is null)
            throw new UserNotFoundException(userDto.TelegramId);

        user.Roles = userDto.Roles;
        user.UpdatedDate = DateTime.UtcNow;
        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.SaveAsync();
        
        return _mapper.Map<UserDto>(user);
    }

    public async Task DeleteUserByTelegramId(long telegramId)
    {
        var user = await _unitOfWork.UserRepository.GetByTelegramIdAsync(telegramId);
        if (user is null)
            throw new UserNotFoundException(telegramId);
        
        await _unitOfWork.UserRepository.DeleteAsync(user);
        await _unitOfWork.SaveAsync();
    }
}