using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Trainings;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Repositories.Trainings;
using Xunit;

namespace NutriFitLogBackend.Tests.Infrastructure.Repositories.Trainings;

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
    public async Task Add_ReturnCreatedExercise(Exercise exercise)
    {
        // Act
        var addedExercise = await _sut.AddAsync(exercise);
        await _dbContext.SaveChangesAsync();
    
        var foundExercise = await _sut.GetByIdAsync(addedExercise.Id);

        // Assert
        foundExercise.Should().NotBeNull();
        foundExercise.Should().BeEquivalentTo(exercise, options => options.Excluding(ex => ex.Id));
    }
    
    
    [Theory]
    [MemberData(nameof(ExerciseData))]
    public async Task GetById_WhenExerciseExists_ReturnExercise(Exercise exercise)
    {
        // Arrange
        _dbContext.Exercises.Add(exercise);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(exercise.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(exercise);
    }
    
    [Theory]
    [MemberData(nameof(ExerciseData))]
    public async Task GetById_WhenExerciseNotExists_ReturnNull(Exercise exercise)
    {
        // Arrange

        // Act
        var result = await _sut.GetByIdAsync(exercise.Id);

        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task GetAll_WhenExercisesExist_ReturnAllExercises()
    {
        // Arrange
        var exercises = new List<Exercise>
        {
            new ("Exercise 1", "Desc 1","Pict 1", ExerciseType.Strength),
            new ("Exercise 2", "Desc 2", "Pict 2", ExerciseType.Running)
        };

        _dbContext.Exercises.AddRange(exercises);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().HaveCount(exercises.Count);
        result.Should().BeEquivalentTo(exercises);
    }
    
    [Theory]
    [MemberData(nameof(ExerciseData))]
    public async Task Update_ShouldBeUpdated(Exercise exercise)
    {
        // Arrange
        _dbContext.Exercises.Add(exercise);
        await _dbContext.SaveChangesAsync();

        // Modify exercise
        exercise.Name = "Updated Name";
        exercise.Description = "Updated Description";

        // Act
        await _sut.UpdateAsync(exercise);
        await _dbContext.SaveChangesAsync();
        var updatedExercise = await _sut.GetByIdAsync(exercise.Id);

        // Assert
        updatedExercise.Name.Should().Be("Updated Name");
        updatedExercise.Description.Should().Be("Updated Description");
    }
    
    [Theory]
    [MemberData(nameof(ExerciseData))]
    public async Task Delete_ShouldBeDeleted(Exercise exercise)
    {
        // Arrange
        _dbContext.Exercises.Add(exercise);
        await _dbContext.SaveChangesAsync();

        // Act
        _sut.DeleteAsync(exercise);
        await _dbContext.SaveChangesAsync();
        var result = await _sut.GetByIdAsync(exercise.Id);

        // Assert
        result.Should().BeNull();
    }
    
    public static IEnumerable<object[]> ExerciseData()
    {
        var fixture = new Fixture();
        var name = fixture.Create<string>();
        var description = fixture.Create<string>();
        var pictureUrl = fixture.Create<string>();
        var type = fixture.Create<ExerciseType>();

        var exercise = new Exercise
        (
            name,
            description,
            pictureUrl,
            type
        );

        yield return new object[] { exercise };
    }
}