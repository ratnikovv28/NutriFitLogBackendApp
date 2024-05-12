using AutoFixture;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Moq;
using NutriFitLogBackend.Application.Services.Trainings;
using NutriFitLogBackend.Domain;
using NutriFitLogBackend.Domain.DTOs.Trainings;
using NutriFitLogBackend.Domain.Entities.Trainings;
using NutriFitLogBackend.Domain.Entities.Users;
using NutriFitLogBackend.Domain.Exceptions;
using NutriFitLogBackend.Domain.Extensions;
using NutriFitLogBackend.Infrastructure.Mapper;
using Xunit;

namespace NutriFitLogBackend.Tests.Application.Services;

public class TrainingServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly IMapper _mapper;

    private readonly TrainingService _sut;
    
    public TrainingServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = new Mapper(config);
        
        _sut = new TrainingService(_uowMock.Object, _mapper);
    }
    
    [Theory]
    [MemberData(nameof(ExercisesData))]
    public async Task GetAllExercises_ReturnsAllExercises(IReadOnlyCollection<Exercise> exercises, IReadOnlyCollection<ExerciseDto> expected)
    {
        // Arrange
        SetupExercisesRepository(exercises);

        // Act
        var result = await _sut.GetAllExercisesAsync();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
        
    [Theory]
    [MemberData(nameof(TrainingData))]
    public async Task GetUserExercisesByDate_WhenUserNotExists_ShouldThrows(IReadOnlyCollection<Training> trainings, User user, DateOnly date, User trainer)
    {
        // Arrange
        SetupUserRepository(user.TelegramId);    
        
        // Act
        var act = () => _sut.GetUserExercisesByDateAsync(user.TelegramId, date);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }
    
    [Theory]
    [MemberData(nameof(TrainingData))]
    public async Task GetUserExercisesByDate_WhenUserNotWorkWithTrainer_ShouldThrows(IReadOnlyCollection<Training> trainings, User user, DateOnly date, User trainer)
    {
        // Arrange
        SetupUserRepository(user.TelegramId, user);     
        
        // Act
        var act = () => _sut.GetUserExercisesByDateAsync(user.TelegramId, date, trainer.TelegramId + 1);

        // Assert
        await act.Should().ThrowAsync<NoAccessUserDataException>();
    }
    
    [Theory]
    [MemberData(nameof(TrainingData))]
    public async Task GetUserExercisesByDate_WhenUserExistsAndHasNoData_ReturnsCreatedEmptyExercises(IReadOnlyCollection<Training> trainings, User user, DateOnly date, User trainer)
    {
        // Arrange
        var training = new Training
        {
            CreatedDate = date.ToDateTimeUtc(),
            Exercises = new List<TrainingExercise>(),
            User = user
        };
        var createdTraining = new Training
        {
            Id = trainings.Last().Id + 1,
            CreatedDate = date.ToDateTimeUtc(),
            Exercises = new List<TrainingExercise>(),
            User = user
        };
        SetupUserRepository(user.TelegramId, user);  
        /*SetupTrainingGetRepository(user.TelegramId, trainings);*/
        SetupTrainingAddRepository(training, createdTraining);
          
        // Act
        var result = await _sut.GetUserExercisesByDateAsync(user.TelegramId, date.AddDays(-1));

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(0);
    }
    
    [Theory]
    [MemberData(nameof(TrainingData))]
    public async Task GetUserExercisesByDate_WhenUserExistsAndHasData_ReturnsAllExercises(IReadOnlyCollection<Training> trainings, User user, DateOnly date, User trainer)
    {
        // Arrange
        user.Trainings.AddRange(trainings);
        SetupUserRepository(user.TelegramId, user);   
        
        // Act
        var result = await _sut.GetUserExercisesByDateAsync(user.TelegramId, date);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(trainings.First(t => t.CreatedDate.ToDateOnly() == date).Exercises.Count);
    }
    
    [Theory]
    [AutoData]
    public async Task CreateExercise_WhenUserNotExists_ShouldThrows(User user, DateOnly date)
    {
        // Arrange
        SetupUserRepository(user.TelegramId);    
        
        // Act
        var act = () => _sut.GetUserExercisesByDateAsync(user.TelegramId, date);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }
    
    [Theory]
    [AutoData]
    public async Task DeleteExercise_WhenUserNotExists_ShouldThrows(User user, DateOnly date)
    {
        // Arrange
        SetupUserRepository(user.TelegramId);    
        
        // Act
        var act = () => _sut.GetUserExercisesByDateAsync(user.TelegramId, date);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }
    
    [Theory]
    [MemberData(nameof(DeleteExerciseData))]
    public async Task DeleteExerciseAsync_ExerciseExists_DeletesSuccessfully(User user, Training training, long exerciseId)
    {
        // Arrange
        SetupUserRepository(user.TelegramId, user);
        SetupTrainingGetRepository(user.TelegramId, new List<Training> { training });

        // Act
        await _sut.DeleteExerciseAsync(user.TelegramId, training.Id, exerciseId);

        // Assert
        _uowMock.Verify(u => u.TrainingExerciseRepository.Delete(It.Is<TrainingExercise>(te => te.ExerciseId == exerciseId)), Times.Once);
        _uowMock.Verify(u => u.SaveAsync(), Times.Once);
    }
    
    public static IEnumerable<object[]> DeleteExerciseData()
    {
        var fixture = new Fixture();
        var telegramId = fixture.Create<long>();
        var user = new User(telegramId);
        var training = new Training
        {
            Id = fixture.Create<long>(),
            User = user,
            Exercises = new List<TrainingExercise>
            {
                new TrainingExercise
                {
                    Id = fixture.Create<long>(),
                    ExerciseId = fixture.Create<long>()
                }
            }
        };
        user.Trainings.Add(training);

        yield return new object[] { user, training, training.Exercises.First().ExerciseId };
    }
    
    public static IEnumerable<object[]> ExercisesData()
    {
        var fixture = new Fixture();
        var exercises = new List<Exercise>();
        int numberOfExercises = fixture.Create<int>() % 5 + 1; 

        for (int i = 0; i < numberOfExercises; i++)
        {
            exercises.Add(new Exercise(
                fixture.Create<string>(),
                fixture.Create<string>(),
                fixture.Create<string>(),
                fixture.Create<ExerciseType>()
            ));
        }
        
        var exercisesDto = exercises.Select(e => new ExerciseDto
        {
            Name = e.Name,
            Description = e.Description,
            PictureUrl = e.PictureUrl,
            Type = e.Type
        }).ToList();
        
        yield return new object[] { exercises, exercisesDto };
    }

    public static IEnumerable<object[]> TrainingData()
    {
        var fixture = new Fixture();
        var trainings = new List<Training>();
        var today = DateTime.UtcNow;
        var trainingsCount = fixture.Create<int>() % 5 + 1;
        
        var telegramId = fixture.Create<long>();
        var user = new User(telegramId);
        var telegramIdTrainer = fixture.Create<long>();
        var trainer = new User(telegramIdTrainer);
        user.Trainers.Add(trainer);
        trainer.Students.Add(user);
        
        for (int i = 0; i < trainingsCount; i++)
        {
            var training = new Training()
            {
                Id = i + 1,
                CreatedDate = today.AddDays(i),
                User = user
            };
            
            var exercisesCount = fixture.Create<int>() % 5 + 1;
            var exercises = new List<TrainingExercise>();
            for (int j = 0; j < exercisesCount; j++)
            {
                var setsCount = fixture.Create<int>() % 5 + 1;
                var sets = new List<Set>();
                for (int k = 0; k < setsCount; k++)
                {
                    sets.Add(new Set()
                    {
                        Id = 1 + i + j + k,
                        Repetitions = fixture.Create<long>()
                    });
                }

                var exercise = new Exercise(
                    fixture.Create<string>(),
                    fixture.Create<string>(),
                    fixture.Create<string>(),
                    fixture.Create<ExerciseType>());
                
                exercises.Add(new TrainingExercise()
                {
                    Id = 1 + i + j,
                    Exercise = exercise,
                    ExerciseId = exercise.Id,
                    Sets = sets,
                    Training = training,
                    TrainingId = training.Id
                });
            }
            
            trainings.Add(new Training()
            {
                Id = i + 1,
                CreatedDate = today.AddDays(i),
                User = user,
                Exercises = exercises
            });
        }
        
        yield return new object[] { trainings, user, today.ToDateOnly(), trainer };
    }
    
    private void SetupUserRepository(long telegramId, User user = null)
    {
        _uowMock.Setup(x => x.UserRepository.GetByTelegramIdAsync(telegramId))
            .ReturnsAsync(user);
    }
    
    private void SetupExercisesRepository(IReadOnlyCollection<Exercise> exercises)
    {
        _uowMock.Setup(x => x.ExercisesRepository.GetAllAsync()).ReturnsAsync(exercises);
    }

    private void SetupTrainingGetRepository(long telegramId, IReadOnlyCollection<Training> trainings)
    {
        _uowMock.Setup(x => x.TrainingRepository.GetAllByTelegramIdAsync(It.Is<long>(t => t == telegramId)))
            .ReturnsAsync(trainings);
    }
    
    private void SetupTrainingAddRepository(Training training, Training createdTraining)
    {
        _uowMock.Setup(x => x.TrainingRepository.AddAsync(It.Is<Training>(t => t == training)))
            .ReturnsAsync(createdTraining);
    }
    
    private void SetupExerciseRepository(long exerciseId, Exercise exercise = null)
    {
        _uowMock.Setup(x => x.ExercisesRepository.GetByIdAsync(exerciseId))
            .ReturnsAsync(exercise);
    }
}