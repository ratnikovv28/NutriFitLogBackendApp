using AutoFixture.Xunit2;
using FluentAssertions;
using NutriFitLogBackend.Controllers.Trainings.Validators;
using NutriFitLogBackend.Domain.DTOs.Trainings.RequestDTOs;
using Xunit;

namespace NutriFitLogBackend.Tests.API.Controllers.Trainings.Validators;

public class DeleteUserExerciseDtoValidatorTests
{
    private readonly DeleteUserExerciseDtoValidator _validator = new();

    [Theory, AutoData]
    public void Validate_WhenAllFieldsAreValid_ShouldNotHaveValidationError(long telegramId, long trainingId, long exerciseId, long trainerId)
    {
        var dto = new DeleteUserExerciseDto
        {
            TelegramId = telegramId,
            TrainingId = trainingId,
            ExerciseId = exerciseId,
            TrainerId = trainerId
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Theory, AutoData]
    public void Validate_WhenTelegramIdIsNegative_ShouldHaveValidationError(long telegramId, long trainingId, long exerciseId, long trainerId)
    {
        var dto = new DeleteUserExerciseDto
        {
            TelegramId = -telegramId,
            TrainingId = trainingId,
            ExerciseId = exerciseId,
            TrainerId = trainerId
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Telegram ID must not be negative"));
    }

    [Theory, AutoData]
    public void Validate_WhenTrainingIdIsNegative_ShouldHaveValidationError(long telegramId, long trainingId, long exerciseId, long trainerId)
    {
        var dto = new DeleteUserExerciseDto
        {
            TelegramId = telegramId,
            TrainingId = -trainingId,
            ExerciseId = exerciseId,
            TrainerId = trainerId
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Training ID must not be negative"));
    }

    [Theory, AutoData]
    public void Validate_WhenExerciseIdIsNegative_ShouldHaveValidationError(long telegramId, long trainingId, long exerciseId, long trainerId)
    {
        var dto = new DeleteUserExerciseDto
        {
            TelegramId = telegramId,
            TrainingId = trainingId,
            ExerciseId = -exerciseId,
            TrainerId = trainerId
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Exercise ID must not be negative"));
    }

    [Theory, AutoData]
    public void Validate_WhenTrainerIdIsNegative_ShouldHaveValidationError(long telegramId, long trainingId, long exerciseId, long trainerId)
    {
        var dto = new DeleteUserExerciseDto
        {
            TelegramId = telegramId,
            TrainingId = trainingId,
            ExerciseId = exerciseId,
            TrainerId = -trainerId
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Trainer ID must not be negative"));
    }
}