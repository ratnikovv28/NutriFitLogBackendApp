using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using NutriFitLogBackend.Domain;
using NutriFitLogBackend.Domain.DTOs.Trainings;
using NutriFitLogBackend.Domain.Entities.Trainings;
using NutriFitLogBackend.Domain.Entities.Users;
using NutriFitLogBackend.Domain.Exceptions;
using NutriFitLogBackend.Domain.Extensions;
using NutriFitLogBackend.Domain.Services.Trainings;

namespace NutriFitLogBackend.Application.Services.Trainings;

public class TrainingService : ITrainingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    [ExcludeFromCodeCoverage]
    public TrainingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    // Получение всех упражнений
    public async Task<IReadOnlyCollection<ExerciseDto>> GetAllExercisesAsync()
    {
        var exercises = await _unitOfWork.ExercisesRepository.GetAllAsync();
        
        return _mapper.Map<IReadOnlyCollection<ExerciseDto>>(exercises);
    }
    
    // Получение подходов упражнения
    public async Task<IReadOnlyCollection<SetDto>> GetExerciseSetsAsync(long trainingId, long exerciseId)
    {
        var exercises = await _unitOfWork.SetRepository.GetByTrainingAndExerciseIdAsync(trainingId, exerciseId);
        
        return _mapper.Map<IReadOnlyCollection<SetDto>>(exercises);
    }
    
    public async Task<IReadOnlyCollection<ExerciseDto>> GetAvailableUserExercisesAsync(long telegramId, long trainingId, long trainerId = 0)
    {
        // Получение пользователя
        var user = await GetUser(telegramId);
        
        // Если пользователь не работает с тренером, то ошибка
        await UserWorkWithTrainerGuard(user, trainerId);
        
        var userTraining = GetTrainingById(user, trainingId);
        var exercises = await _unitOfWork.ExercisesRepository.GetAllAsync();
        var notExistExercises = exercises.Where(e => !userTraining.Exercises.Exists(ue => ue.ExerciseId == e.Id));
        
        return _mapper.Map<IReadOnlyCollection<ExerciseDto>>(notExistExercises);
    }
    
    // Получение информации о тренировке 
    public async Task<TrainingDto> GetUserExercisesByDateAsync(long telegramId, DateOnly date, long trainerId = 0)
    {
        // Получение пользователя
        var user = await GetUser(telegramId);
        
        // Если пользователь не работает с тренером, то ошибка
        await UserWorkWithTrainerGuard(user, trainerId);

        // Тренировка по дню
        var trainingByDate = user.Trainings.FirstOrDefault(t => t.CreatedDate.ToDateOnly() == date);
        
        // Если в указанный день не было тренировки, то нужно создать
        if (trainingByDate is null)
        {
            var training = new Training
            {
                CreatedDate = date.ToDateTimeUtc(),
                Exercises = new List<TrainingExercise>(),
                UserId = user.Id
            };
            trainingByDate = await _unitOfWork.TrainingRepository.AddAsync(training);
            await _unitOfWork.SaveAsync();
        }
        
        // Упражнения в нужный день
        
        return _mapper.Map<TrainingDto>(trainingByDate);
    }
    
    // Добавление упражнения пользователю
    public async Task AddExerciseAsync(long telegramId, long trainingId, long exerciseId, long trainerId = 0)
    {
        // Получение пользователя
        var user = await GetUser(telegramId);

        // Проверки на валидность входных данных
        GetTrainingById(user, trainingId);
        await UserWorkWithTrainerGuard(user, trainerId);
        await ExerciseExistsGuard(exerciseId);
        ExerciseExistsInTrainingGuard(user, trainingId, exerciseId);
        
        var trainingExercise = new TrainingExercise
        {
            ExerciseId = exerciseId,
            Sets = new List<Set>(),
            TrainingId = trainingId
        };
        
        await _unitOfWork.TrainingExerciseRepository.AddAsync(trainingExercise);
        await _unitOfWork.SaveAsync();
    }
    
    // Удаление упражнения из тренировки вместе с повторами
    public async Task DeleteExerciseAsync(long telegramId, long trainingId, long exerciseId, long trainerId = 0)
    {
        // Получение пользователя
        var user = await GetUser(telegramId);

        await UserWorkWithTrainerGuard(user, trainerId);

        var userTrainingExercises = await _unitOfWork.TrainingExerciseRepository.GetByTrainingAndExercideId(trainingId, exerciseId);

        _unitOfWork.TrainingExerciseRepository.Delete(userTrainingExercises);
        await _unitOfWork.SaveAsync();
    }
    
    // Обновление повторов у упражнения
    public async Task UpdateSetsExerciseAsync(long telegramId, long trainingId, long exerciseId, IReadOnlyCollection<SetDto> setsDto, long trainerId = 0)
    {
        // Получение пользователя
        var user = await GetUser(telegramId);

        await UserWorkWithTrainerGuard(user, trainerId);
        GetUserExercisesByTraining(user, trainingId, exerciseId);

        foreach (var setDto in setsDto)
        {
            if (setDto.Id == 0)
            {
                var newSet = new Set
                {
                    Repetitions = setDto.Repetitions,
                    Weight = setDto.Weight,
                    Duration = setDto.Duration,
                    Distance = setDto.Distance,
                    TrainingId = trainingId,
                    ExerciseId = exerciseId
                };
                await _unitOfWork.SetRepository.AddAsync(newSet);
            }
            else
            {
                var updatedSet = new Set
                {
                    Id = setDto.Id,
                    Repetitions = setDto.Repetitions,
                    Weight = setDto.Weight,
                    Duration = setDto.Duration,
                    Distance = setDto.Distance,
                    TrainingId = trainingId,
                    ExerciseId = exerciseId
                };
                _unitOfWork.SetRepository.Update(updatedSet);
            }
        }        
        
        await _unitOfWork.SaveAsync();
    }
    
    // Удаления повторов у упражнения
    public async Task DeleteSetsExerciseAsync(long telegramId, long trainingId, long exerciseId, IReadOnlyCollection<long> setsId, long trainerId = 0)
    {
        // Получение пользователя
        var user = await GetUser(telegramId);

        await UserWorkWithTrainerGuard(user, trainerId);

        // Нахождение упражнение с нужным сетом
        foreach (var setId in setsId)
        {
            var exerciseSet = await _unitOfWork.SetRepository.GetByTrainingAndExerciseIdAndIdAsync(trainingId, exerciseId, setId);
            _unitOfWork.SetRepository.Delete(exerciseSet);
        }
        await _unitOfWork.SaveAsync();
    }

    private async Task<User> GetUser(long telegramId)
    {
        var user = await _unitOfWork.UserRepository.GetByTelegramIdAsync(telegramId);
        if (user == null)
            throw new UserNotFoundException(telegramId);

        return user;
    }

    [ExcludeFromCodeCoverage]
    private async Task UserWorkWithTrainerGuard(User user, long trainerId)
    {
        if(trainerId == 0) return;
        
        if (user is null)
            throw new UserNotFoundException(user.TelegramId);
        var trainer = await _unitOfWork.UserRepository.GetByTelegramIdAsync(trainerId);
        if (trainer is null)
            throw new UserNotFoundException(trainerId);

        var studentTrainer = await _unitOfWork.StudentTrainerRepository.GetRelationShip(user.Id, trainer.Id);
        if (studentTrainer is null)
            throw new StudentTrainerWorkException(
                $"Student with Id = '{user.TelegramId}' doesnt work with Trainer with Id = '{trainerId}'");
    }

    private Training GetTrainingById(User user, long trainingId)
    {
        var userTrainingById = user.Trainings.SingleOrDefault(t => t.Id == trainingId) 
                               ?? throw new NoTrainingUserException(user.TelegramId, trainingId);
        
        return userTrainingById;
    }

    private async Task ExerciseExistsGuard(long exerciseId)
    {
        var exercise = await _unitOfWork.ExercisesRepository.ExistAsync(exerciseId);
        if (exercise == false)
            throw new ExerciseNotFoundException(exerciseId);
    }

    private void ExerciseExistsInTrainingGuard(User user, long trainingId, long exerciseId)
    {
        if (user.Trainings.Single(t => t.Id == trainingId).Exercises
            .Exists(e => e.ExerciseId == exerciseId))
            throw new ExerciseExistsInTrainingException(exerciseId, trainingId);
    }

    private TrainingExercise GetUserExercisesByTraining(User user, long trainingId, long exerciseId)
    {
        var userTrainingById = GetTrainingById(user, trainingId);
        var userTrainingExercises = userTrainingById.Exercises.SingleOrDefault(te => te.ExerciseId == exerciseId)
                                    ?? throw new NoExerciseUserException(user.TelegramId, exerciseId);

        return userTrainingExercises;
    }
}