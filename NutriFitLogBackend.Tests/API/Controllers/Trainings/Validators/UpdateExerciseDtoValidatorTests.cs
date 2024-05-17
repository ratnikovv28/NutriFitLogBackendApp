using AutoFixture.Xunit2;
using FluentAssertions;
using NutriFitLogBackend.Controllers.Trainings.Validators;
using NutriFitLogBackend.Domain.DTOs.Trainings;
using NutriFitLogBackend.Domain.DTOs.Trainings.RequestDTOs;
using Xunit;

namespace NutriFitLogBackend.Tests.API.Controllers.Trainings.Validators;

public class UpdateExerciseDtoValidatorTests
{
    private readonly UpdateExerciseDtoValidator _validator = new();

    [Theory, AutoData]
    public void Validate_WhenAllFieldsAreValid_ShouldNotHaveValidationError(long telegramId, long trainingId, long exerciseId, long trainerId, List<SetDto> sets)
    {
        var dto = new UpdateExerciseDto
        {
            TelegramId = telegramId,
            TrainingId = trainingId,
            ExerciseId = exerciseId,
            TrainerId = trainerId,
            Sets = sets
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenFieldsAreInvalid_ShouldHaveValidationErrors()
    {
        var dto = new UpdateExerciseDto
        {
            TelegramId = 0, 
            TrainingId = 0, 
            ExerciseId = 0, 
            TrainerId = -1, 
            Sets = new List<SetDto> { new SetDto { Id = -1 } } 
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Telegram ID is required"));
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Telegram ID must not be negative"));
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Training ID is required"));
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Training ID must not be negative"));
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Exercise ID is required"));
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Exercise ID must not be negative"));
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Trainer ID must not be negative"));
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Set ID must not be negative"));
    }

    [Fact]
    public void Validate_WhenSetsIsEmpty_ShouldHaveValidationError()
    {
        var dto = new UpdateExerciseDto
        {
            TelegramId = 1,
            TrainingId = 1,
            ExerciseId = 1,
            TrainerId = 1,
            Sets = new List<SetDto>()
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Sets is required"));
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Sets count must be positive"));
    }
}