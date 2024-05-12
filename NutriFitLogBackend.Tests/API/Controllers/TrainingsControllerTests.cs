using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NutriFitLogBackend.Controllers.Trainings;
using NutriFitLogBackend.Domain.DTOs.Trainings;
using NutriFitLogBackend.Domain.Services.Trainings;
using Xunit;

namespace NutriFitLogBackend.Tests.API.Controllers;

public class TrainingsControllerTests
{
    private readonly Mock<ITrainingService> _trainingServiceMock = new Mock<ITrainingService>();
    private readonly TrainingController _controller;

    public TrainingsControllerTests()
    {
        _controller = new TrainingController(_trainingServiceMock.Object);
    }
    
    /*[Theory]
    [MemberData(nameof(ExerciseData))]
    public async Task GetAllExercises_ShouldReturnAllExercises_WhenCalled(List<ExerciseDto> exercises)
    {
        _trainingServiceMock.Setup(service => service.GetAllExercisesAsync()).ReturnsAsync(exercises);

        var result = await _controller.GetAllExercises();

        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(exercises);
    }

    [Theory, AutoData]
    public async Task GetUserExercisesByDate_ShouldReturnExercisesForSpecifiedDateAndUser_WhenCalled(long telegramId, DateOnly date, List<TrainingExerciseDto> exercises)
    {
        _trainingServiceMock.Setup(service => service.GetUserExercisesByDateAsync(telegramId, date, -1)).ReturnsAsync(exercises);

        var result = await _controller.GetUserExercisesByDate(telegramId, date);

        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(exercises);
    }

    [Theory, AutoData]
    public async Task DeleteExercise_ShouldReturnNoContent_WhenExerciseIsDeletedSuccessfully(long telegramId, long trainingId, long exerciseId)
    {
        _trainingServiceMock.Setup(service => service.DeleteExerciseAsync(telegramId, trainingId, exerciseId, -1)).Returns(Task.CompletedTask);

        var result = await _controller.DeleteExercise(telegramId, trainingId, exerciseId);

        result.Should().BeOfType<NoContentResult>();
    }
    
    [Theory, AutoData]
    public async Task CreateExercise_ShouldReturnCreatedResult_WhenExerciseIsCreatedSuccessfully(long telegramId, long trainingId, ExerciseCreateDto exerciseCreateDto)
    {
        _trainingServiceMock.Setup(service => service.CreateExerciseAsync(telegramId, trainingId, exerciseCreateDto.ExerciseId, exerciseCreateDto.TrainerId)).Returns(Task.CompletedTask);

        var result = await _controller.CreateExercise(telegramId, trainingId, exerciseCreateDto);

        result.Result.Should().BeOfType<CreatedAtActionResult>();
        ((CreatedAtActionResult)result.Result).ActionName.Should().Be("GetUserExercisesByDate");
    }

    [Theory, AutoData]
    public async Task UpdateSets_ShouldReturnNoContent_WhenSetsAreUpdatedSuccessfully(long telegramId, long trainingId, long exerciseId, List<SetDto> setsDto)
    {
        _trainingServiceMock.Setup(service => service.UpdateSetsExerciseAsync(telegramId, trainingId, exerciseId, setsDto, -1)).Returns(Task.CompletedTask);

        var result = await _controller.UpdateSets(telegramId, trainingId, exerciseId, setsDto);

        result.Should().BeOfType<NoContentResult>();
    }

    [Theory, AutoData]
    public async Task DeleteSets_ShouldReturnNoContent_WhenSetsAreDeletedSuccessfully(long telegramId, long trainingId, long exerciseId, List<SetDto> setsDto)
    {
        _trainingServiceMock.Setup(service => service.DeleteSetsExerciseAsync(telegramId, trainingId, exerciseId, setsDto, -1)).Returns(Task.CompletedTask);

        var result = await _controller.DeleteSets(telegramId, trainingId, exerciseId, setsDto);

        result.Should().BeOfType<NoContentResult>();
    }
    
    public static IEnumerable<object[]> ExerciseData
    {
        get
        {
            var exercises = new Fixture().CreateMany<ExerciseDto>(5).ToList();
            yield return new object[] { exercises };
        }
    }*/
}