using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.Xunit2;
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
    [MemberData(nameof(TrainingDataForUser))]
    public async Task AddAsync_WhenCalled_AddsTraining(User user, Training training)
    {
        // Arrange

        // Act
        var addedTraining = await _sut.AddAsync(training);
        await _dbContext.SaveChangesAsync();
        var result = await _dbContext.Trainings.FindAsync(addedTraining.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(training);
    }
    
    [ExcludeFromCodeCoverage]
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
                new Set { Repetitions = 10, Weight = 100, Duration = 5, Distance = 100 }
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