using AutoFixture.Xunit2;
using FluentAssertions;
using NutriFitLogBackend.Controllers.Trainings.Validators;
using NutriFitLogBackend.Domain.DTOs.Trainings.RequestDTOs;
using Xunit;

namespace NutriFitLogBackend.Tests.API.Controllers.Trainings.Validators;

public class CreateExerciseDtoValidatorTests
{
    private readonly CreateExerciseDtoValidator _validator = new();

    [Theory, AutoData]
    public void Validate_WhenDataOk_ShouldNotHaveValidationError(long telegramId, long trainingId, long exerciseId, long trainerId)
    {
        var request = new CreateExerciseDto
        {
            TelegramId = telegramId,
            TrainingId = trainingId,
            ExerciseId = exerciseId,
            TrainerId = trainerId
        };

        var result = _validator.Validate(request);
        result.IsValid.Should().BeTrue();
    }
    
    [Theory, AutoData]
    public void Validate_WhenTelegramIdNotPositive_ShouldHaveValidationError(long telegramId, long trainingId, long exerciseId, long trainerId)
    {
        var request = new CreateExerciseDto
        {
            TelegramId = -telegramId,
            TrainingId = trainingId,
            ExerciseId = exerciseId,
            TrainerId = trainerId
        };

        var result = _validator.Validate(request);
        result.IsValid.Should().BeFalse();
    }
    
    [Theory, AutoData]
    public void Validate_WhenTelegramIdEmpty_ShouldHaveValidationError(long telegramId, long trainingId, long exerciseId, long trainerId)
    {
        var request = new CreateExerciseDto
        {
            TrainingId = trainingId,
            ExerciseId = exerciseId,
            TrainerId = trainerId
        };

        var result = _validator.Validate(request);
        result.IsValid.Should().BeFalse();
    }
    
    [Theory, AutoData]
    public void Validate_WhenTrainingIdNotPositive_ShouldHaveValidationError(long telegramId, long trainingId, long exerciseId, long trainerId)
    {
        var request = new CreateExerciseDto
        {
            TelegramId = telegramId,
            TrainingId = -trainingId,
            ExerciseId = exerciseId,
            TrainerId = trainerId
        };

        var result = _validator.Validate(request);
        result.IsValid.Should().BeFalse();
    }
    
    [Theory, AutoData]
    public void Validate_WhenTrainingIdEmpty_ShouldHaveValidationError(long telegramId, long trainingId, long exerciseId, long trainerId)
    {
        var request = new CreateExerciseDto
        {
            TelegramId = telegramId,
            ExerciseId = exerciseId,
            TrainerId = trainerId
        };

        var result = _validator.Validate(request);
        result.IsValid.Should().BeFalse();
    }
    
    [Theory, AutoData]
    public void Validate_WhenExerciseIdNotPositive_ShouldHaveValidationError(long telegramId, long trainingId, long exerciseId, long trainerId)
    {
        var request = new CreateExerciseDto
        {
            TelegramId = telegramId,
            TrainingId = trainingId,
            ExerciseId = -exerciseId,
            TrainerId = trainerId
        };

        var result = _validator.Validate(request);
        result.IsValid.Should().BeFalse();
    }
    
    [Theory, AutoData]
    public void Validate_WhenExerciseIdEmpty_ShouldHaveValidationError(long telegramId, long trainingId, long exerciseId, long trainerId)
    {
        var request = new CreateExerciseDto
        {
            TelegramId = telegramId,
            TrainingId = trainingId,
            TrainerId = trainerId
        };

        var result = _validator.Validate(request);
        result.IsValid.Should().BeFalse();
    }

    [Theory, AutoData]
    public void Validate_WhenTrainerIdNotPositive_ShouldHaveValidationError(long telegramId, long trainingId, long exerciseId, long trainerId)
    {
        var request = new CreateExerciseDto
        {
            TelegramId = telegramId,
            TrainingId = trainingId,
            ExerciseId = exerciseId,
            TrainerId = -trainerId
        };

        var result = _validator.Validate(request);
        result.IsValid.Should().BeFalse();
    }
    
    [Theory, AutoData]
    public void Validate_WhenTrainerIdEmpty_ShouldNotHaveValidationError(long telegramId, long trainingId, long exerciseId, long trainerId)
    {
        var request = new CreateExerciseDto
        {
            TelegramId = telegramId,
            TrainingId = trainingId,
            ExerciseId = exerciseId,
        };

        var result = _validator.Validate(request);
        result.IsValid.Should().BeTrue();
    }
}