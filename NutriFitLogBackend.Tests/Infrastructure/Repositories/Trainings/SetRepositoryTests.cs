using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Trainings;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Repositories.Trainings;
using Xunit;

namespace NutriFitLogBackend.Tests.Infrastructure.Repositories.Trainings;

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
    public async Task Add_ReturnCreatedSet(Set set)
    {
        // Act
        var addedSet = await _sut.AddAsync(set);
        await _dbContext.SaveChangesAsync();
    
        var foundSet = await _sut.GetByIdAsync(addedSet.Id);

        // Assert
        foundSet.Should().NotBeNull();
        foundSet.Should().BeEquivalentTo(set, options => options.Excluding(s => s.Id));
    }
    
    [Theory]
    [MemberData(nameof(SetData))]
    public async Task GetById_WhenSetExists_ReturnSet(Set set)
    {
        // Arrange
        _dbContext.Sets.Add(set);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(set.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(set);
    }
    
    [Theory]
    [MemberData(nameof(SetData))]
    public async Task GetById_WhenSetNotExists_ReturnNull(Set set)
    {
        // Arrange

        // Act
        var result = await _sut.GetByIdAsync(set.Id);

        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task GetAll_WhenSetsExist_ReturnAllSets()
    {
        // Arrange
        var sets = new List<Set>
        {
            new Set { Repetitions = 10, Weight = 100.0, Duration = TimeSpan.FromMinutes(5), Distance = 500 },
            new Set { Repetitions = 8, Weight = 90.0, Duration = TimeSpan.FromMinutes(4), Distance = 400 }
        };

        _dbContext.Sets.AddRange(sets);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().HaveCount(sets.Count);
        result.Should().BeEquivalentTo(sets);
    }
    
    [Theory]
    [MemberData(nameof(SetData))]
    public async Task Update_ShouldBeUpdated(Set set)
    {
        // Arrange
        _dbContext.Sets.Add(set);
        await _dbContext.SaveChangesAsync();

        // Modify set
        set.Repetitions = 20;
        set.Weight = 110.0;

        // Act
        await _sut.UpdateAsync(set);
        await _dbContext.SaveChangesAsync();
        var updatedSet = await _sut.GetByIdAsync(set.Id);

        // Assert
        updatedSet.Repetitions.Should().Be(20);
        updatedSet.Weight.Should().Be(110.0);
    }
    
    [Theory]
    [MemberData(nameof(SetData))]
    public async Task Delete_ShouldBeDeleted(Set set)
    {
        // Arrange
        _dbContext.Sets.Add(set);
        await _dbContext.SaveChangesAsync();

        // Act
        await _sut.DeleteAsync(set);
        await _dbContext.SaveChangesAsync();
        var result = await _sut.GetByIdAsync(set.Id);

        // Assert
        result.Should().BeNull();
    }
    
    public static IEnumerable<object[]> SetData()
    {
        var fixture = new Fixture();
        var repetitions = fixture.Create<long?>();
        var weight = fixture.Create<double?>();
        var duration = fixture.Create<TimeSpan?>();
        var distance = fixture.Create<double?>();

        var set = new Set
        {
            Repetitions = repetitions,
            Weight = weight,
            Duration = duration,
            Distance = distance
        };

        yield return new object[] { set };
    }
}