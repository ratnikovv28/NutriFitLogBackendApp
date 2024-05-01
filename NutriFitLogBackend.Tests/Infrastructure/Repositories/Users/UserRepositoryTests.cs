using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Users;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Repositories.Users;
using Xunit;

namespace NutriFitLogBackend.Tests.Infrastructure.Repositories.Users;

public class UserRepositoryTests
{
    private readonly NutriFitLogContext _dbContext;

    private readonly UserRepository _sut;
    
    public UserRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<NutriFitLogContext>()
            .UseInMemoryDatabase($"{nameof(UserRepositoryTests)}/{Guid.NewGuid()}")
            .Options;

        _dbContext = new NutriFitLogContext(dbContextOptions);

        _sut = new UserRepository(_dbContext);
    }

    [Theory]
    [MemberData(nameof(UserData))]
    public async Task GetByTelegramId_WhenUserExists_ReturnUser(User user)
    {
        // Arrange
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetByTelegramIdAsync(user.TelegramId);

        // Assert
        result.Should().BeEquivalentTo(user);
    }
    
    [Theory]
    [MemberData(nameof(UserData))]
    public async Task GetByTelegramId_WhenUserNotExists_ReturnNull(User user)
    {
        // Arrange

        // Act
        var result = await _sut.GetByTelegramIdAsync(user.Id);

        // Assert
        result.Should().BeNull();
    }
    
    [Theory]
    [MemberData(nameof(UsersData))]
    public async Task GetAll_WhenUsersExists_ReturnUsers(IReadOnlyCollection<User> users)
    {
        // Arrange
        foreach (var user in users)
        {
            _dbContext.Users.Add(user);   
        }
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().Contain(a => users.Contains(a));
        result.Count.Should().Be(users.Count);
    }
    
    [Fact]
    public async Task GetAll_WhenUsersNotExists_ReturnEmptyCollection()
    {
        // Arrange

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
        result.Count.Should().Be(0);
    }
    
    [Theory]
    [AutoData]
    public async Task Add_ReturnCreatedUser(long telegramId)
    {
        // Arrange
        var user = new User(telegramId);
        
        // Act
        await _sut.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        var result = await _sut.GetByTelegramIdAsync(user.TelegramId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Should().BeEquivalentTo(user, options => options.Excluding(u => u.Id));
    }
    
    [Theory]
    [MemberData(nameof(UserData))]
    public async Task Update_ShouldBeUpdated(User user)
    {
        // Arrange
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        
        user.Roles.Add(UserRole.Admin); 
        
        // Act
        await _sut.UpdateAsync(user);
        await _dbContext.SaveChangesAsync();
        var updateUser = await _sut.GetByTelegramIdAsync(user.TelegramId);

        // Assert
        updateUser.Should().BeEquivalentTo(user);
    }
    
    [Theory]
    [MemberData(nameof(UserData))]
    public async Task Delete_ShouldBeDeleted(User user)
    {
        // Arrange
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        
        // Act
        await _sut.DeleteAsync(user);
        await _dbContext.SaveChangesAsync();
        var result = await _sut.GetByTelegramIdAsync(user.TelegramId);

        // Assert
        result.Should().BeNull();
    }
    
    [Theory]
    [MemberData(nameof(UserData))]
    public async Task Exist_WhenExists_ShouldBeTrue(User user)
    {
        // Arrange
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        
        // Act
        var result = await _sut.ExistAsync(user.TelegramId);

        // Assert
        result.Should().BeTrue();
    }
    
    [Theory]
    [MemberData(nameof(UserData))]
    public async Task Exist_WhenNotExists_ShouldBeFalse(User user)
    {
        // Arrange
        
        // Act
        var result = await _sut.ExistAsync(user.TelegramId);

        // Assert
        result.Should().BeFalse();
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
    
    public static IEnumerable<object[]> UsersData()
    {   
        var fixture = new Fixture();
        var usersCount = fixture.Create<long>();

        var users = new List<User>();
        for (int i = 0; i < usersCount; i++)
        {
            var telegramId = fixture.Create<long>();
            users.Add(new User(telegramId));
        }

        yield return new object[]
        {
            users
        };
    }
}