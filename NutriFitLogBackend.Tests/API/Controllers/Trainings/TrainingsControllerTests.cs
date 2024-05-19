using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NutriFitLogBackend.Controllers.Trainings;
using NutriFitLogBackend.Domain.DTOs.Trainings;
using NutriFitLogBackend.Domain.DTOs.Trainings.RequestDTOs;
using NutriFitLogBackend.Domain.Extensions;
using NutriFitLogBackend.Domain.Services.Trainings;
using Xunit;

namespace NutriFitLogBackend.Tests.API.Controllers.Trainings;

public class TrainingControllerTests
{
    private readonly Mock<ITrainingService> _trainingServiceMock;
    private readonly TrainingController _controller;
    private readonly IFixture _fixture;

    public TrainingControllerTests()
    {
        _trainingServiceMock = new Mock<ITrainingService>();
        _controller = new TrainingController(_trainingServiceMock.Object);
        _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
    }

    [Fact]
    public async Task GetAllExercises_WhenCalled_ReturnsOkObjectResultWithExercises()
    {
        // Arrange
        var exercises = _fixture.CreateMany<ExerciseDto>().ToList();
        _trainingServiceMock.Setup(s => s.GetAllExercisesAsync()).ReturnsAsync(exercises);

        // Act
        var result = await _controller.GetAllExercises();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(exercises);
    }

    [Fact]
    public async Task GetAvailableUserExercises_WithValidDto_ReturnsOkObjectResultWithAvailableUserExercises()
    {
        // Arrange
        var dto = _fixture.Create<AvailableUserExerciseDto>();
        var availableExercises = _fixture.CreateMany<ExerciseDto>().ToList();
        _trainingServiceMock.Setup(s => s.GetAvailableUserExercisesAsync(dto.TelegramId, dto.TrainingId, dto.TrainerId)).ReturnsAsync(availableExercises);

        // Act
        var result = await _controller.GetAvailableUserExercises(dto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(availableExercises);
    }

    [Fact]
    public async Task GetUserTrainingByDate_WithValidDto_ReturnsOkObjectResultWithUserTraining()
    {
        // Arrange
        var telegramId = _fixture.Create<long>();
        var trainerId = _fixture.Create<long>();
        var date = _fixture.Create<DateTime>().ToDateOnly();
        var dto = new UserExercisesByDateDto
        {
            TelegramId = telegramId,
            Date = date,
            TrainerId = trainerId
        };
        var training = _fixture.Create<TrainingDto>();
        _trainingServiceMock.Setup(s => s.GetUserExercisesByDateAsync(dto.TelegramId, dto.Date, dto.TrainerId)).ReturnsAsync(training);

        // Act
        var result = await _controller.GetUserTrainingByDate(dto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(training);
    }

    [Fact]
    public async Task GetExerciseSets_WithValidDto_ReturnsOkObjectResultWithExerciseSets()
    {
        // Arrange
        var dto = _fixture.Create<SetExerciseDto>();
        var sets = _fixture.CreateMany<SetDto>().ToList();
        _trainingServiceMock.Setup(s => s.GetExerciseSetsAsync(dto.TrainingId, dto.ExerciseId)).ReturnsAsync(sets);

        // Act
        var result = await _controller.GetExerciseSets(dto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(sets);
    }

    [Theory, AutoData]
    public async Task AddUserExercise_WithValidDto_ReturnsOkResult(CreateExerciseDto dto)
    {
        // Arrange
        _trainingServiceMock.Setup(s => s.AddExerciseAsync(dto.TelegramId, dto.TrainingId, dto.ExerciseId, dto.TrainerId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddUserExercise(dto);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Theory, AutoData]
    public async Task UpdateSetsExercise_WithValidDto_ReturnsOkResult(UpdateExerciseDto dto)
    {
        // Arrange
        _trainingServiceMock.Setup(s => s.UpdateSetsExerciseAsync(dto.TelegramId, dto.TrainingId, dto.ExerciseId, dto.Sets, dto.TrainerId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateSetsExercise(dto);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Theory, AutoData]
    public async Task DeleteUserExercise_WithValidDto_ReturnsOkResult(DeleteUserExerciseDto dto)
    {
        // Arrange
        _trainingServiceMock.Setup(s => s.DeleteExerciseAsync(dto.TelegramId, dto.TrainingId, dto.ExerciseId, dto.TrainerId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteUserExercise(dto);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Theory, AutoData]
    public async Task DeleteSetsExercise_WithValidDto_ReturnsOkResult(DeleteSetsExerciseDto dto)
    {
        // Arrange
        _trainingServiceMock.Setup(s => s.DeleteSetsExerciseAsync(dto.TelegramId, dto.TrainingId, dto.ExerciseId, dto.SetsId, dto.TrainerId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteSetsExercise(dto);

        // Assert
        result.Should().BeOfType<OkResult>();
    }
}