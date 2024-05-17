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

public class TrainingsControllerTests
{
    private readonly Mock<ITrainingService> _mockTrainingService;
    private readonly TrainingController _controller;
    private readonly IFixture _fixture;

    public TrainingsControllerTests()
    {
        _mockTrainingService = new Mock<ITrainingService>();
        _controller = new TrainingController(_mockTrainingService.Object);
        _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
    }
    
    [Theory, AutoData]
    public async Task GetAllExercises_ReturnsOkObjectResult_WithExercises()
    {
        // Arrange
        var exercises = _fixture.Create<IReadOnlyCollection<ExerciseDto>>();
        _mockTrainingService.Setup(x => x.GetAllExercisesAsync()).ReturnsAsync(exercises);

        // Act
        var result = await _controller.GetAllExercises();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(exercises);
    }

    [Theory, AutoData]
    public async Task GetUserTrainingByDate_ReturnsOkObjectResult_WithTraining()
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
        _mockTrainingService.Setup(x => x.GetUserExercisesByDateAsync(telegramId, date, trainerId)).ReturnsAsync(training);

        // Act
        var result = await _controller.GetUserTrainingByDate(dto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(training);
    }
    
    [Theory, AutoData]
    public async Task AddUserExercise_ReturnsOkResult(CreateExerciseDto dto)
    {
        // Arrange
        _mockTrainingService.Setup(x => x.AddExerciseAsync(dto.TelegramId, dto.TrainingId, dto.ExerciseId, dto.TrainerId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddUserExercise(dto);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Theory, AutoData]
    public async Task UpdateSetsExercise_ReturnsOkResult(UpdateExerciseDto dto)
    {
        // Arrange
        _mockTrainingService.Setup(x => x.UpdateSetsExerciseAsync(dto.TelegramId, dto.TrainingId, dto.ExerciseId, dto.Sets, dto.TrainerId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateSetsExercise(dto);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Theory, AutoData]
    public async Task DeleteUserExercise_ReturnsOkResult(DeleteUserExerciseDto dto)
    {
        // Arrange
        _mockTrainingService.Setup(x => x.DeleteExerciseAsync(dto.TelegramId, dto.TrainingId, dto.ExerciseId, dto.TrainerId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteUserExercise(dto);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Theory, AutoData]
    public async Task DeleteSetsExercise_ReturnsOkResult(DeleteSetsExerciseDto dto)
    {
        // Arrange
        _mockTrainingService.Setup(x => x.DeleteSetsExerciseAsync(dto.TelegramId, dto.TrainingId, dto.ExerciseId, dto.SetId, dto.TrainerId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteSetsExercise(dto);

        // Assert
        result.Should().BeOfType<OkResult>();
    }
}