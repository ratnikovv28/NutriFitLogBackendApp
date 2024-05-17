using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NutriFitLogBackend.Controllers.Users;
using NutriFitLogBackend.Domain.DTOs.Users;
using NutriFitLogBackend.Domain.DTOs.Users.RequestDTOs;
using NutriFitLogBackend.Domain.Services.Users;
using Xunit;

namespace NutriFitLogBackend.Tests.API.Controllers.Users;

public class UserControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly UserController _controller;
    private readonly IFixture _fixture;

    public UserControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UserController(_mockUserService.Object);
        _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
    }

    [Theory, AutoData]
    public async Task CreateUser_ReturnsExceptedResponse(CreateUserDto dto)
    {
        // Arrange
        var createdUser = _fixture.Create<UserDto>();
        _mockUserService.Setup(x => x.CreateUser(dto)).ReturnsAsync(createdUser);

        // Act
        var result = await _controller.CreateUser(dto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(createdUser);
    }
    
    [Theory, AutoData]
    public async Task Get_ReturnsOkObjectResult_WithUser(long telegramId)
    {
        // Arrange
        var user = _fixture.Create<UserDto>();
        _mockUserService.Setup(x => x.GetUserByTelegramId(telegramId)).ReturnsAsync(user);

        // Act
        var result = await _controller.Get(telegramId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsOkObjectResult_WithUsers()
    {
        // Arrange
        var users = _fixture.CreateMany<UserDto>().ToList();
        _mockUserService.Setup(x => x.GetUsers()).ReturnsAsync(users);

        // Act
        var result = await _controller.GetAllUsers();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(users);
    }

    [Theory, AutoData]
    public async Task UpdateUser_ReturnsOkObjectResult_WithUpdatedUser(UpdateUserDto dto)
    {
        // Arrange
        var updatedUser = _fixture.Create<UserDto>();
        _mockUserService.Setup(x => x.UpdateUser(dto)).ReturnsAsync(updatedUser);

        // Act
        var result = await _controller.UpdateUser(dto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(updatedUser);
    }

    [Theory, AutoData]
    public async Task Delete_ReturnsOkResult(long telegramId)
    {
        // Arrange
        _mockUserService.Setup(x => x.DeleteUserByTelegramId(telegramId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(telegramId);

        // Assert
        result.Should().BeOfType<OkResult>();
    }
}