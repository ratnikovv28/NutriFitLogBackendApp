using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using FluentAssertions;
using Moq;
using AutoFixture.Xunit2;
using NutriFitLogBackend.Application.Services.Trainings;
using NutriFitLogBackend.Domain;
using NutriFitLogBackend.Domain.DTOs.Trainings;
using NutriFitLogBackend.Domain.Entities.Trainings;
using NutriFitLogBackend.Domain.Entities.Users;
using NutriFitLogBackend.Domain.Exceptions;
using NutriFitLogBackend.Domain.Extensions;
using NutriFitLogBackend.Infrastructure.Mapper;
using Xunit;

public class TrainingServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly IMapper _mapper;
    private readonly TrainingService _sut;
    private readonly IFixture _fixture;

    public TrainingServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = new Mapper(config);
        _sut = new TrainingService(_uowMock.Object, _mapper);
        _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
    }

    [Theory]
    [MemberData(nameof(ExerciseData))]
    public async Task GetAllExercisesAsync_ReturnsAllExercises(Exercise exercise)
    {
        // Arrange
        var exercises = new List<Exercise>();
        exercises.Add(exercise);
        _uowMock.Setup(u => u.ExercisesRepository.GetAllAsync()).ReturnsAsync(exercises);

        // Act
        var result = await _sut.GetAllExercisesAsync();

        // Assert
        result.Should().BeEquivalentTo(_mapper.Map<IReadOnlyCollection<ExerciseDto>>(exercises));
    }

    [Theory]
    [MemberData(nameof(SetData))]
    public async Task GetExerciseSetsAsync_ReturnsExerciseSets(Set set)
    {
        // Arrange
        var sets = new List<Set>();
        sets.Add(set);
        var trainingId = _fixture.Create<long>();
        var exerciseId = _fixture.Create<long>();
        _uowMock.Setup(u => u.SetRepository.GetByTrainingAndExerciseIdAsync(trainingId, exerciseId)).ReturnsAsync(sets);

        // Act
        var result = await _sut.GetExerciseSetsAsync(trainingId, exerciseId);

        // Assert
        result.Should().BeEquivalentTo(_mapper.Map<IReadOnlyCollection<SetDto>>(sets));
    }

    [Theory, AutoData]
    public async Task GetAvailableUserExercisesAsync_WhenUserDoesNotExist_ThrowsUserNotFoundException(long telegramId, long trainingId, long trainerId)
    {
        // Arrange
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.GetAvailableUserExercisesAsync(telegramId, trainingId, 0);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Theory, AutoData]
    public async Task GetAvailableUserExercisesAsync_WhenUserDoesNotWorkWithTrainer_ThrowsStudentTrainerWorkException(long telegramId, long trainingId, long trainerId)
    {
        // Arrange
        var user = new User(telegramId)
        {
            Id = telegramId + 1
        };
        var trainer = new User(trainerId)
        {
            Id = trainerId + 1
        };
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(user.TelegramId)).ReturnsAsync(user);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(trainerId)).ReturnsAsync(trainer);
        _uowMock.Setup(u => u.StudentTrainerRepository.GetRelationShip(telegramId + 1,trainerId + 1)).ReturnsAsync((StudentTrainer)null);

        // Act
        Func<Task> act = async () => await _sut.GetAvailableUserExercisesAsync(user.TelegramId, trainingId, trainerId);

        // Assert
        await act.Should().ThrowAsync<StudentTrainerWorkException>();
    }

    [Theory]
    [MemberData(nameof(AvailableExerciseData))]
    public async Task GetAvailableUserExercisesAsync_ReturnsAvailableExercises(User user, Training training, List<Exercise> exercises)
    {
        // Arrange
        var trainingId = training.Id;
        training.Exercises = exercises.Select(e => new TrainingExercise { ExerciseId = e.Id + 1 }).ToList();
        user.Trainings.Add(training);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(user.TelegramId)).ReturnsAsync(user);
        _uowMock.Setup(u => u.ExercisesRepository.GetAllAsync()).ReturnsAsync(exercises);

        // Act
        var result = await _sut.GetAvailableUserExercisesAsync(user.TelegramId, trainingId);

        // Assert
        result.Should().BeEquivalentTo(_mapper.Map<IReadOnlyCollection<ExerciseDto>>(exercises));
    }

    [Theory, AutoData]
    public async Task GetUserExercisesByDateAsync_WhenUserDoesNotExist_ThrowsUserNotFoundException(long telegramId, DateTime date, long trainerId)
    {
        // Arrange
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.GetUserExercisesByDateAsync(telegramId, date.ToDateOnly(), trainerId);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Theory, AutoData]
    public async Task GetUserExercisesByDateAsync_ReturnsTraining(long telegramId, DateTime date)
    {
        // Arrange
        var user = new User(telegramId);
        var training = new Training { CreatedDate = date };
        user.Trainings.Add(training);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(user.TelegramId)).ReturnsAsync(user);

        // Act
        var result = await _sut.GetUserExercisesByDateAsync(user.TelegramId, date.ToDateOnly());

        // Assert
        result.Should().BeEquivalentTo(_mapper.Map<TrainingDto>(training));
    }
    
    [Theory, AutoData]
    public async Task GetUserExercisesByDateAsync_WhenTrainingDoesNotExist_ReturnsTraining(long telegramId, DateTime date)
    {
        // Arrange
        var user = new User(telegramId);
        var training = new Training { CreatedDate = date.AddDays(1) };
        var expectedTraining = new Training { CreatedDate = date };
        user.Trainings.Add(training);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(user.TelegramId)).ReturnsAsync(user);
        _uowMock.Setup(u => u.TrainingRepository.AddAsync(It.IsAny<Training>())).ReturnsAsync(expectedTraining);

        // Act
        var result = await _sut.GetUserExercisesByDateAsync(user.TelegramId, date.ToDateOnly());

        // Assert
        result.Should().BeEquivalentTo(_mapper.Map<TrainingDto>(expectedTraining));
    }

    [Theory, AutoData]
    public async Task AddExerciseAsync_WhenUserDoesNotExist_ThrowsUserNotFoundException(long telegramId, long trainingId, long exerciseId, long trainerId)
    {
        // Arrange
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.AddExerciseAsync(telegramId, trainingId, exerciseId, trainerId);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Theory, AutoData]
    public async Task AddExerciseAsync_WhenExerciseAlreadyExist_AddsExercise(long telegramId, long trainingId, long exerciseId)
    {
        // Arrange
        var user = new User(telegramId);
        user.Trainings.Add(new Training()
        {
            Id = trainingId,
            Exercises = new List<TrainingExercise>()
            {
                new TrainingExercise()
                {
                    ExerciseId = exerciseId,
                    TrainingId = trainingId
                }
            }
        });
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync(user);
        _uowMock.Setup(u => u.ExercisesRepository.ExistAsync(exerciseId)).ReturnsAsync(true);
        _uowMock.Setup(u => u.TrainingExerciseRepository.AddAsync(It.IsAny<TrainingExercise>())).ReturnsAsync(new TrainingExercise());

        // Act
        var act = () => _sut.AddExerciseAsync(telegramId, trainingId, exerciseId);

        // Assert
        await act.Should().ThrowAsync<ExerciseExistsInTrainingException>();
    }
    
    [Theory, AutoData]
    public async Task AddExerciseAsync_WhenExerciseNotFound_ShouldThrows(long telegramId, long trainingId, long exerciseId)
    {
        // Arrange
        var user = new User(telegramId);
        user.Trainings.Add(new Training()
        {
            Id = trainingId,
            Exercises = new List<TrainingExercise>()
            {
                new TrainingExercise()
                {
                    ExerciseId = exerciseId,
                    TrainingId = trainingId
                }
            }
        });
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync(user);
        _uowMock.Setup(u => u.ExercisesRepository.ExistAsync(exerciseId)).ReturnsAsync(false);
        _uowMock.Setup(u => u.TrainingExerciseRepository.AddAsync(It.IsAny<TrainingExercise>())).ReturnsAsync(new TrainingExercise());

        // Act
        var act = () => _sut.AddExerciseAsync(telegramId, trainingId, exerciseId);

        // Assert
        await act.Should().ThrowAsync<ExerciseNotFoundException>();
    }
    
    [Theory, AutoData]
    public async Task AddExerciseAsync_AddsExercise(long telegramId, long trainingId, long exerciseId)
    {
        // Arrange
        var user = new User(telegramId);
        user.Trainings.Add(new Training()
        {
            Id = trainingId
        });
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync(user);
        _uowMock.Setup(u => u.ExercisesRepository.ExistAsync(exerciseId)).ReturnsAsync(true);
        _uowMock.Setup(u => u.TrainingExerciseRepository.AddAsync(It.IsAny<TrainingExercise>())).ReturnsAsync(new TrainingExercise());

        // Act
        await _sut.AddExerciseAsync(telegramId, trainingId, exerciseId);

        // Assert
        _uowMock.Verify(u => u.TrainingExerciseRepository.AddAsync(It.Is<TrainingExercise>(te => te.ExerciseId == exerciseId && te.TrainingId == trainingId)), Times.Once);
        _uowMock.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Theory, AutoData]
    public async Task DeleteExerciseAsync_WhenUserDoesNotExist_ThrowsUserNotFoundException(long telegramId, long trainingId, long exerciseId, long trainerId)
    {
        // Arrange
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.DeleteExerciseAsync(telegramId, trainingId, exerciseId, trainerId);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Theory]
    [MemberData(nameof(DeleteExerciseData))]
    public async Task DeleteExerciseAsync_DeletesExercise(long telegramId, long trainingId, long exerciseId, long trainerId, User user, TrainingExercise trainingExercise)
    {
        // Arrange
        var trainer = new User(trainerId)
        {
            Id = trainerId + 1
        };
        user.Id = telegramId + 1;
        var studentTrainer = new StudentTrainer()
        {
            Id = telegramId + 1,
            IsWorking = true,
            StudentId = telegramId,
            Student = user,
            TrainerId = trainerId,
            Trainer = trainer
        };
        user.Trainers.Add(studentTrainer);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync(user);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(trainerId)).ReturnsAsync(trainer);
        _uowMock.Setup(u => u.StudentTrainerRepository.GetRelationShip(telegramId + 1,trainerId + 1)).ReturnsAsync(studentTrainer);
        _uowMock.Setup(u => u.TrainingExerciseRepository.GetByTrainingAndExercideId(trainingId, exerciseId)).ReturnsAsync(trainingExercise);

        // Act
        await _sut.DeleteExerciseAsync(telegramId, trainingId, exerciseId, trainerId);

        // Assert
        _uowMock.Verify(u => u.TrainingExerciseRepository.Delete(It.Is<TrainingExercise>(te => te.Id == trainingExercise.Id)), Times.Once);
        _uowMock.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Theory, AutoData]
    public async Task UpdateSetsExerciseAsync_WhenUserDoesNotExist_ThrowsUserNotFoundException(long telegramId, long trainingId, long exerciseId, IReadOnlyCollection<SetDto> setsDto, long trainerId)
    {
        // Arrange
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.UpdateSetsExerciseAsync(telegramId, trainingId, exerciseId, setsDto, trainerId);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Theory]
    [MemberData(nameof(UpdateSetsData))]
    public async Task UpdateSetsExerciseAsync_UpdatesSets(long telegramId, long trainingId, long exerciseId, List<SetDto> setsDto, TrainingExercise trainingExercise)
    {
        // Arrange
        var user = new User(telegramId);
        user.Trainings.Add(new Training()
        {
            Id = trainingId,
            Exercises = new List<TrainingExercise>()
            {
                new TrainingExercise()
                {
                    ExerciseId = exerciseId,
                    TrainingId = trainingId
                }
            }
        });
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync(user);
        _uowMock.Setup(u => u.TrainingExerciseRepository.GetByTrainingAndExercideId(trainingId, exerciseId)).ReturnsAsync(trainingExercise);
        _uowMock.Setup(u => u.SetRepository.Update(It.IsAny<Set>()));

        // Act
        await _sut.UpdateSetsExerciseAsync(telegramId, trainingId, exerciseId, setsDto);

        // Assert
        foreach (var setDto in setsDto)
        {
            _uowMock.Verify(u => u.SetRepository.Update(It.IsAny<Set>()), Times.Exactly(setsDto.Count));
        }
        _uowMock.Verify(u => u.SaveAsync(), Times.Once);
    }
    
    [Theory]
    [MemberData(nameof(UpdateSetsZeroIdData))]
    public async Task UpdateSetsExerciseAsync_WhenSetIdIsZero_UpdatesSets(long telegramId, long trainingId, long exerciseId, List<SetDto> setsDto, TrainingExercise trainingExercise)
    {
        // Arrange
        var user = new User(telegramId);
        user.Trainings.Add(new Training()
        {
            Id = trainingId,
            Exercises = new List<TrainingExercise>()
            {
                new TrainingExercise()
                {
                    ExerciseId = exerciseId,
                    TrainingId = trainingId
                }
            }
        });
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync(user);
        _uowMock.Setup(u => u.TrainingExerciseRepository.GetByTrainingAndExercideId(trainingId, exerciseId)).ReturnsAsync(trainingExercise);
        _uowMock.Setup(u => u.SetRepository.AddAsync(It.IsAny<Set>())).ReturnsAsync((Set)null);

        // Act
        await _sut.UpdateSetsExerciseAsync(telegramId, trainingId, exerciseId, setsDto);

        // Assert
        foreach (var setDto in setsDto)
        {
            _uowMock.Verify(u => u.SetRepository.AddAsync(It.IsAny<Set>()), Times.Exactly(setsDto.Count));
        }
        _uowMock.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Theory, AutoData]
    public async Task DeleteSetsExerciseAsync_WhenUserDoesNotExist_ThrowsUserNotFoundException(long telegramId, long trainingId, long exerciseId, IReadOnlyCollection<long> setsId, long trainerId)
    {
        // Arrange
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.DeleteSetsExerciseAsync(telegramId, trainingId, exerciseId, setsId, trainerId);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Theory]
    [MemberData(nameof(DeleteSetsData))]
    public async Task DeleteSetsExerciseAsync_DeletesSets(long telegramId, long trainingId, long exerciseId, List<long> setsId, User user, Set set)
    {
        // Arrange
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync(user);
        foreach (var setId in setsId)
        {
            _uowMock.Setup(u => u.SetRepository.GetByTrainingAndExerciseIdAndIdAsync(trainingId, exerciseId, setId)).ReturnsAsync(set);
        }

        // Act
        await _sut.DeleteSetsExerciseAsync(telegramId, trainingId, exerciseId, setsId);

        // Assert
        foreach (var setId in setsId)
        {
            _uowMock.Verify(u => u.SetRepository.Delete(It.IsAny<Set>()), Times.Once);
        }
        _uowMock.Verify(u => u.SaveAsync(), Times.Once);
    }
    
    [ExcludeFromCodeCoverage]
    public static IEnumerable<object[]> SetData()
    {
        var fixture = new Fixture();
        var exerciseId = fixture.Create<long>();
        var trainingId = fixture.Create<long>();
        
        var set = new Set()
        {
            Repetitions = 10,
            Weight = 100,
            Duration = 5, 
            Distance = 100,
            ExerciseId = exerciseId,
            TrainingId = trainingId,
            Id = fixture.Create<long>()
        };


        yield return new object[]
        {
            set
        };
    }
    
    [ExcludeFromCodeCoverage]
    public static IEnumerable<object[]> ExerciseData()
    {
        var fixture = new Fixture();
        var exercise = new Exercise(
            name: fixture.Create<string>(),
            description: fixture.Create<string>(),
            pictureUrl: fixture.Create<string>(),
            type: fixture.Create<ExerciseType>()
        );

        yield return new object[]
        {
            exercise
        };
    }
    
    [ExcludeFromCodeCoverage]
    public static IEnumerable<object[]> UpdateSetsData()
    {
        var fixture = new Fixture();
        var sets = fixture.CreateMany<SetDto>().ToList();
        var exerciseId = fixture.Create<long>();
        var trainingId = fixture.Create<long>();
        var setId = fixture.Create<long>();
        var exercise = new Exercise(
            name: fixture.Create<string>(),
            description: fixture.Create<string>(),
            pictureUrl: fixture.Create<string>(),
            type: fixture.Create<ExerciseType>()
        )
        {
            Id = exerciseId
        };

        var trainingExercise = new TrainingExercise
        {
            Id = exerciseId + 1,
            Exercise = exercise,
            TrainingId = trainingId,
            ExerciseId = exerciseId,
            Sets = new List<Set>
            {
                new Set
                {
                    Id = setId,
                    Repetitions = 10, Weight = 100, Duration = 5, Distance = 100
                }
            }
        };

        yield return new object[]
        {
            fixture.Create<long>(),
            trainingId,
            exerciseId,
            sets,
            trainingExercise
        };
    }
    
    [ExcludeFromCodeCoverage]
    public static IEnumerable<object[]> UpdateSetsZeroIdData()
    {
        var fixture = new Fixture();
        var sets = fixture.CreateMany<SetDto>().ToList();
        foreach (var set in sets)
        {
            set.Id = 0;
        }
        var exerciseId = fixture.Create<long>();
        var trainingId = fixture.Create<long>();
        var setId = fixture.Create<long>();
        var exercise = new Exercise(
            name: fixture.Create<string>(),
            description: fixture.Create<string>(),
            pictureUrl: fixture.Create<string>(),
            type: fixture.Create<ExerciseType>()
        )
        {
            Id = exerciseId
        };

        var trainingExercise = new TrainingExercise
        {
            Id = exerciseId + 1,
            Exercise = exercise,
            TrainingId = trainingId,
            ExerciseId = exerciseId,
            Sets = new List<Set>
            {
                new Set
                {
                    Id = 0,
                    Repetitions = 10, Weight = 100, Duration = 5, Distance = 100
                }
            }
        };

        yield return new object[]
        {
            fixture.Create<long>(),
            trainingId,
            exerciseId,
            sets,
            trainingExercise
        };
    }
    
    [ExcludeFromCodeCoverage]
    public static IEnumerable<object[]> DeleteSetsData()
    {
        var fixture = new Fixture();
        var telegramId = fixture.Create<long>();
        var user = new User(telegramId);
        var exerciseId = fixture.Create<long>();
        var trainingId = fixture.Create<long>();
        var setId = fixture.Create<long>();
        var setIds = new List<long>()
        {
            setId
        };
        var exercise = new Exercise(
            name: fixture.Create<string>(),
            description: fixture.Create<string>(),
            pictureUrl: fixture.Create<string>(),
            type: fixture.Create<ExerciseType>()
        )
        {
            Id = exerciseId
        };

        var set = new Set
        {
            Id = setId,
            Repetitions = 10, Weight = 100, Duration = 5, Distance = 100
        };
        
        var trainingExercise = new TrainingExercise
        {
            Id = exerciseId + 1,
            Exercise = exercise,
            TrainingId = trainingId,
            ExerciseId = exerciseId,
            Sets = new List<Set>
            {
                new Set
                {
                    Id = setId,
                    Repetitions = 10, Weight = 100, Duration = 5, Distance = 100
                }
            }
        };

        yield return new object[]
        {
            telegramId,
            trainingId,
            exerciseId,
            setIds,
            user,
            set
        };
    }
    
    [ExcludeFromCodeCoverage]
    public static IEnumerable<object[]> DeleteExerciseData()
    {
        var fixture = new Fixture();
        var telegramId = fixture.Create<long>();
        var trainerId = fixture.Create<long>();
        var user = new User(telegramId);
        var exerciseId = fixture.Create<long>();
        var trainingId = fixture.Create<long>();
        var setId = fixture.Create<long>();
        var setIds = new List<long>()
        {
            setId
        };
        var exercise = new Exercise(
            name: fixture.Create<string>(),
            description: fixture.Create<string>(),
            pictureUrl: fixture.Create<string>(),
            type: fixture.Create<ExerciseType>()
        )
        {
            Id = exerciseId
        };

        var set = new Set
        {
            Id = setId,
            Repetitions = 10, Weight = 100, Duration = 5, Distance = 100
        };
        
        var trainingExercise = new TrainingExercise
        {
            Id = exerciseId + 1,
            Exercise = exercise,
            TrainingId = trainingId,
            ExerciseId = exerciseId,
            Sets = new List<Set>
            {
                new Set
                {
                    Id = setId,
                    Repetitions = 10, Weight = 100, Duration = 5, Distance = 100
                }
            }
        };

        yield return new object[]
        {
            telegramId,
            trainingId,
            exerciseId,
            trainerId,
            user,
            trainingExercise
        };
    }
    
    [ExcludeFromCodeCoverage]
    public static IEnumerable<object[]> AvailableExerciseData()
    {
        var fixture = new Fixture();
        var telegramId = fixture.Create<long>();
        var trainerId = fixture.Create<long>();
        var user = new User(telegramId);
        var exerciseId = fixture.Create<long>();
        var trainingId = fixture.Create<long>();
        var setId = fixture.Create<long>();
        var setIds = new List<long>()
        {
            setId
        };
        var exercises = new List<Exercise>();
        var exercise = new Exercise(
            name: fixture.Create<string>(),
            description: fixture.Create<string>(),
            pictureUrl: fixture.Create<string>(),
            type: fixture.Create<ExerciseType>()
        )
        {
            Id = exerciseId
        };

        var newexercise = new Exercise(
            name: fixture.Create<string>(),
            description: fixture.Create<string>(),
            pictureUrl: fixture.Create<string>(),
            type: fixture.Create<ExerciseType>()
        )
        {
            Id = 1
        };
        
        exercises.Add(newexercise);

        var set = new Set
        {
            Id = setId,
            Repetitions = 10, Weight = 100, Duration = 5, Distance = 100
        };
        
        var trainingExercise = new TrainingExercise
        {
            Id = exerciseId + 1,
            Exercise = exercise,
            TrainingId = trainingId,
            ExerciseId = exerciseId,
            Sets = new List<Set>
            {
                new Set
                {
                    Id = setId,
                    Repetitions = 10, Weight = 100, Duration = 5, Distance = 100
                }
            }
        };

        var training = new Training
        {
            CreatedDate = DateTime.UtcNow,
            User = user,
            Exercises = new List<TrainingExercise> { trainingExercise }
        };
        
        yield return new object[]
        {
            user,
            training,
            exercises
        };
    }
}
