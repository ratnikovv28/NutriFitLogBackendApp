using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Users;
using NutriFitLogBackend.Infrastructure.Database;
using NutriFitLogBackend.Infrastructure.Repositories.Users;
using Xunit;

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
    public async Task GetByIdAsync_WhenUserExists_ReturnUser(User user)
    {
        // Arrange
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(user.Id);

        // Assert
        result.Should().BeEquivalentTo(user);
    }

    [Theory]
    [MemberData(nameof(UserData))]
    public async Task GetByIdAsync_WhenUserNotExists_ReturnNull(User user)
    {
        // Arrange

        // Act
        var result = await _sut.GetByIdAsync(user.Id);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(UserData))]
    public async Task GetJustByTelegramIdAsync_WhenUserExists_ReturnUser(User user)
    {
        // Arrange
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetJustByTelegramIdAsync(user.TelegramId);

        // Assert
        result.Should().BeEquivalentTo(user);
    }

    [Theory]
    [MemberData(nameof(UserData))]
    public async Task GetJustByTelegramIdAsync_WhenUserNotExists_ReturnNull(User user)
    {
        // Arrange

        // Act
        var result = await _sut.GetJustByTelegramIdAsync(user.TelegramId);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(UserData))]
    public async Task GetByTelegramIdAsync_WhenUserExists_ReturnUser(User user)
    {
        // Arrange
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sut.GetByTelegramIdAsync(user.TelegramId);

        // Assert
        result.Should().BeEquivalentTo(user, options => options
            .Excluding(u => u.Trainings)
            .Excluding(u => u.Meals)
            .Excluding(u => u.Students)
            .Excluding(u => u.Trainers));
    }

    [Theory]
    [MemberData(nameof(UserData))]
    public async Task GetByTelegramIdAsync_WhenUserNotExists_ReturnNull(User user)
    {
        // Arrange

        // Act
        var result = await _sut.GetByTelegramIdAsync(user.TelegramId);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [AutoData]
    public async Task GetAllAsync_WhenUsersExist_ReturnUsers(IReadOnlyCollection<long> telegramIds)
    {
        // Arrange
        var users = new List<User>();
        foreach (var telegramId in telegramIds)
        {
            users.Add(new User(telegramId));
        }
        
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
    public async Task GetAllAsync_WhenUsersNotExist_ReturnEmptyCollection()
    {
        // Arrange

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
        result.Count.Should().Be(0);
    }

    [Theory, AutoData]
    public async Task AddAsync_ReturnCreatedUser(long telegramId)
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
    public async Task UpdateAsync_ShouldBeUpdated(User user)
    {
        // Arrange
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        _sut.UpdateAsync(user);
        await _dbContext.SaveChangesAsync();
        var updatedUser = await _sut.GetByTelegramIdAsync(user.TelegramId);

        // Assert
        updatedUser.Should().BeEquivalentTo(user);
    }

    [Theory]
    [MemberData(nameof(UserData))]
    public async Task DeleteAsync_ShouldBeDeleted(User user)
    {
        // Arrange
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        _sut.DeleteAsync(user);
        await _dbContext.SaveChangesAsync();
        var result = await _sut.GetByTelegramIdAsync(user.TelegramId);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(UserData))]
    public async Task ExistAsync_WhenExists_ShouldBeTrue(User user)
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
    public async Task ExistAsync_WhenNotExists_ShouldBeFalse(User user)
    {
        // Arrange

        // Act
        var result = await _sut.ExistAsync(user.TelegramId);

        // Assert
        result.Should().BeFalse();
    }

    [ExcludeFromCodeCoverage]
    public static IEnumerable<object[]> UserData()
    {
        var fixture = new Fixture();
        var user = fixture.Build<User>()
            .Without(u => u.Trainings)
            .Without(u => u.Meals)
            .Without(u => u.Students)
            .Without(u => u.Trainers)
            .Create();

        yield return new object[] { user };
    }

    [ExcludeFromCodeCoverage]
    public static IEnumerable<object[]> UsersData()
    {
        var fixture = new Fixture();
        var users = fixture.CreateMany<User>(5)
            .Select(u => fixture.Build<User>()
                .Without(x => x.Trainings)
                .Without(x => x.Meals)
                .Without(x => x.Students)
                .Without(x => x.Trainers)
                .Create())
            .ToList();

        yield return new object[] { users };
    }
}