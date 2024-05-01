using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Trainings;
using NutriFitLogBackend.Domain.Entities.Users;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Repositories.Trainings;
using Xunit;

namespace NutriFitLogBackend.Tests.Infrastructure.Repositories.Trainings;

public class TrainingRepositoryTests
{
    private readonly NutriFitLogContext _dbContext;
    private readonly TrainingRepository _sut;

    public TrainingRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<NutriFitLogContext>()
            .UseInMemoryDatabase($"{nameof(TrainingRepositoryTests)}/{Guid.NewGuid()}")
            .Options;

        _dbContext = new NutriFitLogContext(dbContextOptions);
        _sut = new TrainingRepository(_dbContext);
    }
        
    [Theory]
    [MemberData(nameof(TrainingData))]
    public async Task Add_ReturnCreatedTraining(Training training)
    {
        // Act
        var addedTraining = await _sut.AddAsync(training);
        await _dbContext.SaveChangesAsync();

        var foundTraining = await _sut.GetByIdAsync(addedTraining.Id);

        // Assert
        foundTraining.Should().NotBeNull();
        foundTraining.Should().BeEquivalentTo(training, options => options
            .Excluding(t => t.Id)
            .Excluding(t => t.User.Id)
            .ExcludingNestedObjects());
    }
    
    [Theory]
    [MemberData(nameof(TrainingDataForUser))]
    public async Task GetAllByUserId_WhenTrainingsExist_ReturnTrainingsForUser(User user, Training training)
    {
        // Arrange
        _dbContext.Trainings.Add(training);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllByTelegramIdAsync(user.TelegramId);

        // Assert
        result.Should().NotBeEmpty();
        result.All(t => t.User.TelegramId == user.TelegramId).Should().BeTrue();
    }
    
    [Theory]
    [MemberData(nameof(TrainingDataForUser))]
    public async Task GetAllByUserId_WhenTrainingsNotExist_ReturnEmptyCollection(User user, Training training)
    {
        // Arrange
        _dbContext.Trainings.Add(training);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllByTelegramIdAsync(user.TelegramId + 1);

        // Assert
        result.Should().BeEmpty();
        result.Count.Should().Be(0);
    }

    [Fact]
    public async Task GetAllByUserId_WhenNoTrainingsExist_ReturnEmpty()
    {
        // Act
        var result = await _sut.GetAllByTelegramIdAsync(999);

        // Assert
        result.Should().BeEmpty();
    }
    
    [Theory]
    [MemberData(nameof(TrainingData))]
    public async Task GetById_WhenTrainingExists_ReturnTraining(Training training)
    {
        // Arrange
        _dbContext.Trainings.Add(training);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(training.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(training, options => options
            .Excluding(t => t.User.Id)
            .ExcludingNestedObjects());
    }
    
    [Theory]
    [MemberData(nameof(TrainingData))]
    public async Task GetById_WhenTrainingNotExists_ReturnNull(Training training)
    {
        // Arrange

        // Act
        var result = await _sut.GetByIdAsync(training.Id);

        // Assert
        result.Should().BeNull();
    }
    
    [Theory]
    [MemberData(nameof(TrainingData))]
    public async Task Update_ShouldBeUpdated(Training training)
    {
        // Arrange
        _dbContext.Trainings.Add(training);
        await _dbContext.SaveChangesAsync();

        // Modify training
        training.Exercises.First().Sets.First().Repetitions = 20;

        // Act
        await _sut.UpdateAsync(training);
        await _dbContext.SaveChangesAsync();
        var updatedTraining = await _sut.GetByIdAsync(training.Id);

        // Assert
        updatedTraining.Exercises.First().Sets.First().Repetitions.Should().Be(20);
    }
    
    [Theory]
    [MemberData(nameof(TrainingData))]
    public async Task Delete_ShouldBeDeleted(Training training)
    {
        // Arrange
        _dbContext.Trainings.Add(training);
        await _dbContext.SaveChangesAsync();

        // Act
        await _sut.DeleteAsync(training);
        await _dbContext.SaveChangesAsync();
        var result = await _sut.GetByIdAsync(training.Id);

        // Assert
        result.Should().BeNull();
    }
    
    public static IEnumerable<object[]> TrainingData()
    {
        var fixture = new Fixture();
        var user = new User(fixture.Create<long>());
        var exercise = new Exercise(
            name: fixture.Create<string>(),
            description: fixture.Create<string>(),
            pictureUrl: fixture.Create<string>(),
            type: fixture.Create<ExerciseType>()
        );

        var trainingExercise = new TrainingExercise
        {
            Exercise = exercise,
            Sets = new List<Set>
            {
                new Set { Repetitions = 10, Weight = 100, Duration = TimeSpan.FromMinutes(5), Distance = 100 }
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
            training
        };
    }
    
    public static IEnumerable<object[]> TrainingDataForUser()
    {
        var fixture = new Fixture();
        var user = new User(fixture.Create<long>());
        var exercise = new Exercise(
            name: fixture.Create<string>(),
            description: fixture.Create<string>(),
            pictureUrl: fixture.Create<string>(),
            type: fixture.Create<ExerciseType>()
        );

        var trainingExercise = new TrainingExercise
        {
            Exercise = exercise,
            Sets = new List<Set>
            {
                new Set { Repetitions = 10, Weight = 100, Duration = TimeSpan.FromMinutes(5), Distance = 100 }
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
            training
        };
    }
}