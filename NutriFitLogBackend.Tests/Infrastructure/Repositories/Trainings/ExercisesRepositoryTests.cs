using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Trainings;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Repositories.Trainings;
using Xunit;

public class ExercisesRepositoryTests
{
    private readonly NutriFitLogContext _dbContext;
    private readonly ExercisesRepository _sut;

    public ExercisesRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<NutriFitLogContext>()
            .UseInMemoryDatabase($"{nameof(ExercisesRepositoryTests)}/{Guid.NewGuid()}")
            .Options;

        _dbContext = new NutriFitLogContext(dbContextOptions);
        _sut = new ExercisesRepository(_dbContext);
    }

    [Theory]
    [MemberData(nameof(ExerciseData))]
    public async Task GetByIdAsync_WhenExists_ReturnsExercise(Exercise exercise)
    {
        // Arrange
        await _dbContext.Exercises.AddAsync(exercise);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(exercise.Id);

        // Assert
        result.Should().BeEquivalentTo(exercise);
    }

    [Theory]
    [AutoData]
    public async Task GetByIdAsync_WhenNotExists_ReturnsNull(long exerciseId)
    {
        // Arrange

        // Act
        var result = await _sut.GetByIdAsync(exerciseId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_WhenExercisesExist_ReturnsExercises()
    {
        // Arrange
        var exercises = new List<Exercise>
        {
            new Exercise("Exercise1", "Desc1", "Pic1", ExerciseType.Running),
            new Exercise("Exercise2", "Desc2", "Pic2", ExerciseType.Strength),
        };

        await _dbContext.Exercises.AddRangeAsync(exercises);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(exercises);
    }

    [Fact]
    public async Task GetAllAsync_WhenExercisesNotExist_ReturnsEmptyCollection()
    {
        // Arrange

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(ExerciseData))]
    public async Task ExistAsync_WhenExists_ReturnsTrue(Exercise exercise)
    {
        // Arrange
        await _dbContext.Exercises.AddAsync(exercise);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.ExistAsync(exercise.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [AutoData]
    public async Task ExistAsync_WhenNotExists_ReturnsFalse(long exerciseId)
    {
        // Arrange

        // Act
        var result = await _sut.ExistAsync(exerciseId);

        // Assert
        result.Should().BeFalse();
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
}