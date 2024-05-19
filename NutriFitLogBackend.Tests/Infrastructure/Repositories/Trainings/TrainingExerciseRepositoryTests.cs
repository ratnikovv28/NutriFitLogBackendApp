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

public class TrainingExerciseRepositoryTests
{
    private readonly NutriFitLogContext _dbContext;
    private readonly TrainingExerciseRepository _sut;

    public TrainingExerciseRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<NutriFitLogContext>()
            .UseInMemoryDatabase($"{nameof(TrainingExerciseRepositoryTests)}/{Guid.NewGuid()}")
            .Options;

        _dbContext = new NutriFitLogContext(dbContextOptions);
        _sut = new TrainingExerciseRepository(_dbContext);
    }

    [Theory]
    [MemberData(nameof(TrainingExerciseData))]
    public async Task AddAsync_WhenCalled_AddsTrainingExercise(TrainingExercise trainingExercise)
    {
        // Arrange

        // Act
        var addedTrainingExercise = await _sut.AddAsync(trainingExercise);
        await _dbContext.SaveChangesAsync();
        var result = await _dbContext.TrainingExercise.FindAsync(addedTrainingExercise.TrainingId, addedTrainingExercise.ExerciseId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(trainingExercise);
    }

    [Theory]
    [MemberData(nameof(TrainingExerciseData))]
    public async Task Delete_WhenCalled_DeletesTrainingExercise(TrainingExercise trainingExercise)
    {
        // Arrange
        await _dbContext.TrainingExercise.AddAsync(trainingExercise);
        await _dbContext.SaveChangesAsync();

        // Act
        _sut.Delete(trainingExercise);
        await _dbContext.SaveChangesAsync();
        var result = await _dbContext.TrainingExercise.FindAsync(trainingExercise.TrainingId, trainingExercise.ExerciseId);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(TrainingExerciseData))]
    public async Task GetByTrainingAndExercideId_WhenExists_ReturnTrainingExercise(TrainingExercise trainingExercise)
    {
        // Arrange
        await _dbContext.TrainingExercise.AddAsync(trainingExercise);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetByTrainingAndExercideId(trainingExercise.TrainingId, trainingExercise.ExerciseId);

        // Assert
        result.Should().BeEquivalentTo(trainingExercise);
    }

    [Theory, AutoData]
    public async Task GetByTrainingAndExercideId_WhenNotExists_ReturnNull(long trainingId, long exerciseId)
    {
        // Arrange

        // Act
        var result = await _sut.GetByTrainingAndExercideId(trainingId, exerciseId);

        // Assert
        result.Should().BeNull();
    }
    
    [ExcludeFromCodeCoverage]
    public static IEnumerable<object[]> TrainingExerciseData()
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
            TrainingId = fixture.Create<long>(),
            ExerciseId = fixture.Create<long>(),
            Sets = new List<Set>
            {
                new Set { Repetitions = 10, Weight = 100, Duration = 5, Distance = 100 }
            }
        };

        yield return new object[]
        {
            trainingExercise
        };
    }
}
