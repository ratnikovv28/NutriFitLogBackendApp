using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Users;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Repositories.Users;
using Xunit;

namespace NutriFitLogBackend.Tests.Infrastructure.Repositories.Users;

public class StudentTrainerRepositoryTests
{
    private readonly NutriFitLogContext _dbContext;
    private readonly StudentTrainerRepository _sut;

    public StudentTrainerRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<NutriFitLogContext>()
            .UseInMemoryDatabase($"{nameof(StudentTrainerRepositoryTests)}/{Guid.NewGuid()}")
            .Options;

        _dbContext = new NutriFitLogContext(dbContextOptions);
        _sut = new StudentTrainerRepository(_dbContext);
    }

    [Theory]
    [MemberData(nameof(StudentTrainerData))]
    public async Task GetRelationShip_WhenRelationExists_ReturnRelation(StudentTrainer studentTrainer)
    {
        // Arrange
        _dbContext.StudentTrainer.Add(studentTrainer);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetRelationShip(studentTrainer.StudentId, studentTrainer.TrainerId);

        // Assert
        result.Should().BeEquivalentTo(studentTrainer);
    }

    [Theory]
    [MemberData(nameof(StudentTrainerData))]
    public async Task GetRelationShip_WhenRelationNotExists_ReturnNull(StudentTrainer studentTrainer)
    {
        // Arrange

        // Act
        var result = await _sut.GetRelationShip(studentTrainer.StudentId, studentTrainer.TrainerId);

        // Assert
        result.Should().BeNull();
    }

    [Theory, AutoData]
    public async Task AddRelationShip_ReturnCreatedRelation(long studentId, long trainerId, bool isWorking)
    {
        // Arrange
        var studentTrainer = new StudentTrainer
        {
            StudentId = studentId,
            TrainerId = trainerId,
            IsWorking = isWorking
        };

        // Act
        await _sut.AddRelationShip(studentTrainer);
        await _dbContext.SaveChangesAsync();
        var result = await _sut.GetRelationShip(studentTrainer.StudentId, studentTrainer.TrainerId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(studentTrainer);
    }

    [Theory]
    [MemberData(nameof(StudentTrainerData))]
    public async Task UpdateRelationShip_ShouldBeUpdated(StudentTrainer studentTrainer)
    {
        // Arrange
        _dbContext.StudentTrainer.Add(studentTrainer);
        await _dbContext.SaveChangesAsync();

        studentTrainer.IsWorking = true; 

        // Act
        _sut.UpdateRelationShip(studentTrainer);
        await _dbContext.SaveChangesAsync();
        var updatedRelation = await _sut.GetRelationShip(studentTrainer.StudentId, studentTrainer.TrainerId);

        // Assert
        updatedRelation.Should().BeEquivalentTo(studentTrainer);
    }

    [Theory]
    [MemberData(nameof(StudentTrainerData))]
    public async Task DeleteRelationShip_ShouldBeDeleted(StudentTrainer studentTrainer)
    {
        // Arrange
        _dbContext.StudentTrainer.Add(studentTrainer);
        await _dbContext.SaveChangesAsync();

        // Act
        _sut.DeleteRelationShip(studentTrainer);
        await _dbContext.SaveChangesAsync();
        var result = await _sut.GetRelationShip(studentTrainer.StudentId, studentTrainer.TrainerId);

        // Assert
        result.Should().BeNull();
    }

    [ExcludeFromCodeCoverage]
    public static IEnumerable<object[]> StudentTrainerData()
    {
        var fixture = new Fixture();
        var studentId = fixture.Create<long>();
        var trainerId = fixture.Create<long>();

        var studentTrainer = new StudentTrainer
        {
            StudentId = studentId,
            TrainerId = trainerId,
            IsWorking = true
        };

        yield return new object[] { studentTrainer };
    }
}