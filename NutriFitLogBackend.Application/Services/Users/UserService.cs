using AutoMapper;
using NutriFitLogBackend.Domain;
using NutriFitLogBackend.Domain.DTOs.Users;
using NutriFitLogBackend.Domain.DTOs.Users.RequestDTOs;
using NutriFitLogBackend.Domain.Entities.Users;
using NutriFitLogBackend.Domain.Exceptions;
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
        _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.SaveAsync();
        
        return _mapper.Map<UserDto>(user);
    }

    public async Task DeleteUserByTelegramId(long telegramId)
    {
        var user = await _unitOfWork.UserRepository.GetByTelegramIdAsync(telegramId);
        if (user is null)
            throw new UserNotFoundException(telegramId);
        
        _unitOfWork.UserRepository.DeleteAsync(user);
        await _unitOfWork.SaveAsync();
    }
    
    public async Task<IReadOnlyCollection<UserDto>> GetStudents(long telegramId, bool activeStudents)
    {   
        var user = await _unitOfWork.UserRepository.GetByTelegramIdAsync(telegramId);
        if (user is null)
            throw new UserNotFoundException(telegramId);

        var students = new List<User>();
        var userStudents = user.Students.Where(st => st.IsWorking == activeStudents);
        foreach (var userStudent in userStudents)
        {
            students.Add(await _unitOfWork.UserRepository.GetByIdAsync(userStudent.StudentId));
        }  
        
        return _mapper.Map<IReadOnlyCollection<UserDto>>(students);
    }
    
    public async Task<IReadOnlyCollection<UserDto>> GetTrainers(long telegramId)
    {   
        var user = await _unitOfWork.UserRepository.GetByTelegramIdAsync(telegramId);
        if (user is null)
            throw new UserNotFoundException(telegramId);

        var trainerWhoWorkingWithUser = user.Trainers.Where(t => t.IsWorking == true);
        
        var trainers = new List<User>();
        foreach (var studentTrainer in trainerWhoWorkingWithUser)
        {
            trainers.Add(await _unitOfWork.UserRepository.GetByIdAsync(studentTrainer.TrainerId));
        }        
        
        return _mapper.Map<IReadOnlyCollection<UserDto>>(trainers);
    }
    
    public async Task<UserDto> UpdateTrainerStatus(long telegramId)
    {   
        var user = await _unitOfWork.UserRepository.GetJustByTelegramIdAsync(telegramId);
        if (user is null)
            throw new UserNotFoundException(telegramId);

        user.IsActiveTrainer = !user.IsActiveTrainer;
        await _unitOfWork.SaveAsync();

        return _mapper.Map<UserDto>(user);
    }
    
    public async Task DeleteStudentTrainerRelationShip(long studentId, long trainerId)
    {   
        var student = await _unitOfWork.UserRepository.GetByTelegramIdAsync(studentId);
        if (student is null)
            throw new UserNotFoundException(studentId);
        var trainer = await _unitOfWork.UserRepository.GetByTelegramIdAsync(trainerId);
        if (trainer is null)
            throw new UserNotFoundException(trainerId);

        var studentTrainer = await _unitOfWork.StudentTrainerRepository.GetRelationShip(student.Id, trainer.Id);
        _unitOfWork.StudentTrainerRepository.DeleteRelationShip(studentTrainer);
        await _unitOfWork.SaveAsync();
    }
    
    public async Task AddStudentToTrainer(long studentId, long trainerId)
    {  
        var trainer = await _unitOfWork.UserRepository.GetByTelegramIdAsync(trainerId);
        if (trainer is null)
            throw new UserNotFoundException(trainerId);
        
        if (trainer.IsActiveTrainer is false)
            throw new Exception($"Trainer with ID = '{trainerId}' doesnt work right now");
        
        var student = await _unitOfWork.UserRepository.GetByTelegramIdAsync(studentId);
        if (student is null)
            throw new UserNotFoundException(studentId);
        
        var studentTrainer = await _unitOfWork.StudentTrainerRepository.GetRelationShip(student.Id, trainer.Id);
        if (studentTrainer is not null)
            throw new Exception(
            $"Relationship between Student with ID = '{studentId}' and Trainer with ID = '{trainerId}' already exists");

        studentTrainer = new StudentTrainer()
        {
            StudentId = student.Id,
            TrainerId = trainer.Id
        };
        
        await _unitOfWork.StudentTrainerRepository.AddRelationShip(studentTrainer);
        await _unitOfWork.SaveAsync();
    }
    
    public async Task CreateStudentTrainer(long studentId, long trainerId)
    {   
        var student = await _unitOfWork.UserRepository.GetByTelegramIdAsync(studentId);
        if (student is null)
            throw new UserNotFoundException(studentId);
        var trainer = await _unitOfWork.UserRepository.GetByTelegramIdAsync(trainerId);
        if (trainer is null)
            throw new UserNotFoundException(trainerId);
        
        var studentTrainer = await _unitOfWork.StudentTrainerRepository.GetRelationShip(student.Id, trainer.Id);
        if (studentTrainer is null)
            throw new Exception(
                $"Relationship between Student with ID = '{studentId}' and Trainer with ID = '{trainerId}' not exists");

        studentTrainer.IsWorking = true;
        
        _unitOfWork.StudentTrainerRepository.UpdateRelationShip(studentTrainer);
        await _unitOfWork.SaveAsync();
    }
}