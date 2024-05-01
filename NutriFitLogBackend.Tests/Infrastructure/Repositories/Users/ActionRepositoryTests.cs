using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Users;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Repositories.Users;
using Xunit;
using Action = NutriFitLogBackend.Domain.Entities.Users.Action;

namespace NutriFitLogBackend.Tests.Infrastructure.Repositories.Users;

public class ActionRepositoryTests
{
    private readonly NutriFitLogContext _dbContext;

    private readonly ActionRepository _sut;

    public ActionRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<NutriFitLogContext>()
            .UseInMemoryDatabase($"{nameof(UserRepositoryTests)}/{Guid.NewGuid()}")
            .Options;
        
        _dbContext = new NutriFitLogContext(dbContextOptions);

        _sut = new ActionRepository(_dbContext);
    }
    
    [Theory]
    [AutoData]
    public async Task Add_ReturnCreatedAction(long telegramId, string description)
    {
        // Arrange
        var admin = new User(telegramId);
        _dbContext.Users.Add(admin);
        await _dbContext.SaveChangesAsync();

        var action = new Action(description, admin);
        
        // Act
        await _sut.AddAsync(action);
        await _dbContext.SaveChangesAsync();
        var result = await _sut.GetByIdAsync(action.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.AdminId.Should().Be(admin.Id);
        result.Description.Should().Be(description);
        result.Should().BeEquivalentTo(action, options => options
            .Excluding(a => a.Id)
            .Excluding(a => a.Admin));
    }
    
    [Theory]
    [AutoData]
    public async Task GetById_WhenActionExists_ReturnAction(long telegramId, string description)
    {
        // Arrange
        var admin = new User(telegramId);
        _dbContext.Users.Add(admin);
        await _dbContext.SaveChangesAsync();

        var action = new Action(description, admin);
        await _sut.AddAsync(action);
        await _dbContext.SaveChangesAsync();
        
        // Act
        var result = await _sut.GetByIdAsync(action.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.AdminId.Should().Be(admin.Id);
        result.Description.Should().Be(description);
        result.Should().BeEquivalentTo(action, options => options
            .Excluding(a => a.Id)
            .Excluding(a => a.Admin));
    }
    
    [Theory]
    [AutoData]
    public async Task GetById_WhenActionNotExists_ReturnNull(long id)
    {
        // Arrange
        
        // Act
        var result = await _sut.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }
        
    public static IEnumerable<object[]> UserData()
    {   
        var fixture = new Fixture();
        var telegramId = fixture.Create<long>();
        
        var user = new User(telegramId);

        yield return new object[]
        {
            user
        };
    }
    
    public static IEnumerable<object[]> ActionData()
    {   
        var fixture = new Fixture();
        var telegramId = fixture.Create<long>();
        var description = fixture.Create<string>();
        
        var user = new User(telegramId);
        var action = new Action(description, user);

        yield return new object[]
        {
            action
        };
    }
}