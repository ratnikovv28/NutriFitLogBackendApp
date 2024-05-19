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

using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class UserControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly UserController _controller;
    private readonly IFixture _fixture;

    public UserControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _controller = new UserController(_userServiceMock.Object);
        _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
    }

    [Fact]
    public async Task Get_WhenCalledWithValidId_ReturnsOkObjectResultWithUser()
    {
        // Arrange
        var user = _fixture.Create<UserDto>();
        var telegramId = user.TelegramId;
        _userServiceMock.Setup(s => s.GetUserByTelegramId(telegramId)).ReturnsAsync(user);

        // Act
        var result = await _controller.Get(telegramId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GetTrainers_WhenCalledWithValidId_ReturnsOkObjectResultWithTrainers()
    {
        // Arrange
        var trainers = _fixture.CreateMany<UserDto>().ToList();
        var telegramId = _fixture.Create<long>();
        _userServiceMock.Setup(s => s.GetTrainers(telegramId)).ReturnsAsync(trainers);

        // Act
        var result = await _controller.GetTrainers(telegramId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(trainers);
    }

    [Fact]
    public async Task CreateUser_WithValidDto_ReturnsOkObjectResultWithCreatedUser()
    {
        // Arrange
        var createUserDto = _fixture.Create<CreateUserDto>();
        var createdUser = _fixture.Create<UserDto>();
        _userServiceMock.Setup(s => s.CreateUser(createUserDto)).ReturnsAsync(createdUser);

        // Act
        var result = await _controller.CreateUser(createUserDto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(createdUser);
    }

    [Fact]
    public async Task GetStudents_WithValidDto_ReturnsOkObjectResultWithStudents()
    {
        // Arrange
        var dto = _fixture.Create<TrainerStudentsDto>();
        var students = _fixture.CreateMany<UserDto>().ToList();
        _userServiceMock.Setup(s => s.GetStudents(dto.TelegramId, dto.AreActive)).ReturnsAsync(students);

        // Act
        var result = await _controller.GetStudents(dto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(students);
    }

    [Theory, AutoData]
    public async Task AddStudentTrainer_WithValidDto_ReturnsOkResult(StudentTrainerDto dto)
    {
        // Arrange
        _userServiceMock.Setup(s => s.AddStudentToTrainer(dto.StudentTelegramId, dto.TrainerTelegramId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddStudentTrainer(dto);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Theory, AutoData]
    public async Task CreateStudentTrainer_WithValidDto_ReturnsOkResult(StudentTrainerDto dto)
    {
        // Arrange
        _userServiceMock.Setup(s => s.CreateStudentTrainer(dto.StudentTelegramId, dto.TrainerTelegramId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateStudentTrainer(dto);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task UpdateUserTrainerStatus_WithValidDto_ReturnsOkObjectResultWithUpdatedUser()
    {
        // Arrange
        var createUserDto = _fixture.Create<CreateUserDto>();
        var updatedUser = _fixture.Create<UserDto>();
        _userServiceMock.Setup(s => s.UpdateTrainerStatus(createUserDto.TelegramId)).ReturnsAsync(updatedUser);

        // Act
        var result = await _controller.UpdateUserTrainerStatus(createUserDto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(updatedUser);
    }

    [Theory, AutoData]
    public async Task Delete_WithValidDto_ReturnsOkResult(StudentTrainerDto dto)
    {
        // Arrange
        _userServiceMock.Setup(s => s.DeleteStudentTrainerRelationShip(dto.StudentTelegramId, dto.TrainerTelegramId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(dto);

        // Assert
        result.Should().BeOfType<OkResult>();
    }
}