using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Trainings;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Repositories.Trainings;
using Xunit;

public class SetRepositoryTests
{
    private readonly NutriFitLogContext _dbContext;
    private readonly SetRepository _sut;

    public SetRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<NutriFitLogContext>()
            .UseInMemoryDatabase($"{nameof(SetRepositoryTests)}/{Guid.NewGuid()}")
            .Options;

        _dbContext = new NutriFitLogContext(dbContextOptions);
        _sut = new SetRepository(_dbContext);
    }

    [Theory]
    [MemberData(nameof(SetData))]
    public async Task GetByIdAsync_WhenExists_ReturnsSet(long trainingId, long exerciseId,Set set)
    {
        // Arrange
        await _dbContext.Sets.AddAsync(set);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(set.Id);

        // Assert
        result.Should().BeEquivalentTo(set);
    }

    [Theory]
    [AutoData]
    public async Task GetByIdAsync_WhenNotExists_ReturnsNull(long setId)
    {
        // Arrange

        // Act
        var result = await _sut.GetByIdAsync(setId);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(SetData))]
    public async Task GetByTrainingAndExerciseIdAsync_WhenSetsExist_ReturnsSets(long trainingId, long exerciseId, Set set)
    {
        // Arrange
        await _dbContext.Sets.AddRangeAsync(new []{set});
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetByTrainingAndExerciseIdAsync(trainingId, exerciseId);

        // Assert
        result.Should().BeEquivalentTo(new []{set});
    }

    [Theory]
    [AutoData]
    public async Task GetByTrainingAndExerciseIdAsync_WhenSetsNotExist_ReturnsEmptyCollection(long trainingId, long exerciseId)
    {
        // Arrange

        // Act
        var result = await _sut.GetByTrainingAndExerciseIdAsync(trainingId, exerciseId);

        // Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(SetData))]
    public async Task GetByTrainingAndExerciseIdAndIdAsync_WhenExists_ReturnsSet(long trainingId, long exerciseId, Set set)
    {
        // Arrange
        set.TrainingId = trainingId;
        set.ExerciseId = exerciseId;
        await _dbContext.Sets.AddAsync(set);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetByTrainingAndExerciseIdAndIdAsync(trainingId, exerciseId, set.Id);

        // Assert
        result.Should().BeEquivalentTo(set);
    }

    [Theory]
    [AutoData]
    public async Task GetByTrainingAndExerciseIdAndIdAsync_WhenNotExists_ReturnsNull(long trainingId, long exerciseId, long setId)
    {
        // Arrange

        // Act
        var result = await _sut.GetByTrainingAndExerciseIdAndIdAsync(trainingId, exerciseId, setId);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(SetData))]
    public async Task AddAsync_WhenCalled_AddsSet(long trainingId, long exerciseId,Set set)
    {
        // Arrange

        // Act
        var addedSet = await _sut.AddAsync(set);
        await _dbContext.SaveChangesAsync();
        var result = await _dbContext.Sets.FindAsync(addedSet.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(set);
    }

    [Theory]
    [MemberData(nameof(SetData))]
    public async Task Update_WhenCalled_UpdatesSet(long trainingId, long exerciseId,Set set)
    {
        // Arrange
        await _dbContext.Sets.AddAsync(set);
        await _dbContext.SaveChangesAsync();

        set.Repetitions = set.Repetitions + 1;

        // Act
        _sut.Update(set);
        await _dbContext.SaveChangesAsync();
        var updatedSet = await _sut.GetByIdAsync(set.Id);

        // Assert
        updatedSet.Should().BeEquivalentTo(set);
    }

    [Theory]
    [MemberData(nameof(SetData))]
    public async Task Delete_WhenCalled_DeletesSet(long trainingId, long exerciseId,Set set)
    {
        // Arrange
        await _dbContext.Sets.AddAsync(set);
        await _dbContext.SaveChangesAsync();

        // Act
        _sut.Delete(set);
        await _dbContext.SaveChangesAsync();
        var result = await _sut.GetByIdAsync(set.Id);

        // Assert
        result.Should().BeNull();
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
            trainingId,
            exerciseId,
            set
        };
    }
}
