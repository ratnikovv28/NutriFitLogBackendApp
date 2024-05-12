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
    
    // Получение информации о тренировке 
    public async Task<TrainingDto> GetUserExercisesByDateAsync(long telegramId, DateOnly date, long trainerId = 0)
    {
        // Получение пользователя
        var user = await GetUser(telegramId);
        
        // Если пользователь не работает с тренером, то ошибка
        UserWorkWithTrainerGuard(user, trainerId);

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
        UserWorkWithTrainerGuard(user, trainerId);
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

        UserWorkWithTrainerGuard(user, trainerId);

        var userTrainingExercises = GetUserExercisesByTraining(user, trainingId, exerciseId);

        _unitOfWork.TrainingExerciseRepository.Delete(userTrainingExercises);
        await _unitOfWork.SaveAsync();
    }
    
    // Обновление повторов у упражнения
    public async Task UpdateSetsExerciseAsync(long telegramId, long trainingId, long exerciseId, IReadOnlyCollection<SetDto> setsDto, long trainerId = 0)
    {
        // Получение пользователя
        var user = await GetUser(telegramId);

        UserWorkWithTrainerGuard(user, trainerId);
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
    public async Task DeleteSetsExerciseAsync(long telegramId, long trainingId, long exerciseId, long setId, long trainerId = 0)
    {
        // Получение пользователя
        var user = await GetUser(telegramId);

        UserWorkWithTrainerGuard(user, trainerId);

        // Получение упражнений пользователя по тренировке
        var userTrainingExercises = GetUserExercisesByTraining(user, trainingId, exerciseId);
        // Нахождение упражнение с нужным сетом
        var exerciseSet = GetSetFromExercise(userTrainingExercises, user, setId);
        
        _unitOfWork.SetRepository.Delete(exerciseSet);
        await _unitOfWork.SaveAsync();
    }

    private Set GetSetFromExercise(TrainingExercise userTrainingExercises, User user, long setId)
    {
        var exerciseSet = userTrainingExercises.Sets.FirstOrDefault(s => s.Id == setId);
        if (exerciseSet is null)
            throw new NoSetUserException(user.TelegramId, setId);

        return exerciseSet;
    }
    
    private async Task<User> GetUser(long telegramId)
    {
        var user = await _unitOfWork.UserRepository.GetByTelegramIdAsync(telegramId);
        if (user == null)
            throw new UserNotFoundException(telegramId);

        return user;
    }

    private void UserWorkWithTrainerGuard(User user, long trainerId)
    {
        if(trainerId != 0 && user.Trainers.All(u => u.TelegramId != trainerId))
            throw new NoAccessUserDataException(user.TelegramId, trainerId);
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