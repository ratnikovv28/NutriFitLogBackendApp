using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Nutrition;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Repositories.Nutrition;
using Xunit;

namespace NutriFitLogBackend.Tests.Infrastructure.Repositories.Nutrition;

public class DayPartRepositoryTests
{
    private readonly NutriFitLogContext _dbContext;
    private readonly DayPartRepository _sut;

    public DayPartRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<NutriFitLogContext>()
            .UseInMemoryDatabase($"{nameof(DayPartRepositoryTests)}/{Guid.NewGuid()}")
            .Options;

        _dbContext = new NutriFitLogContext(dbContextOptions);
        _sut = new DayPartRepository(_dbContext);
    }
    
    [Fact]
    public async Task GetAll_WhenDayPartsExist_ReturnAllDayParts()
    {
        // Arrange
        var dayParts = new List<DayPart>
        {
            new DayPart { Name = "Morning" },
            new DayPart { Name = "Afternoon" },
            new DayPart { Name = "Evening" }
        };

        _dbContext.DayParts.AddRange(dayParts);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().HaveCount(dayParts.Count);
        result.Should().BeEquivalentTo(dayParts, options => options.ComparingByMembers<DayPart>());
    }
    
    [Fact]
    public async Task GetAll_WhenNoDayPartsExist_ReturnEmptyCollection()
    {
        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }
}