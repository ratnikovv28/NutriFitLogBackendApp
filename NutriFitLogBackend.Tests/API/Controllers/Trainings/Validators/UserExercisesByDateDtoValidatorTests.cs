using AutoFixture.Xunit2;
using FluentAssertions;
using NutriFitLogBackend.Controllers.Trainings.Validators;
using NutriFitLogBackend.Domain.DTOs.Trainings.RequestDTOs;
using Xunit;

namespace NutriFitLogBackend.Tests.API.Controllers.Trainings.Validators;

public class UserExercisesByDateDtoValidatorTests
{
    private readonly UserExercisesByDateDtoValidator _validator = new();

    [Theory, AutoData]
    public void Validate_WhenAllFieldsAreValid_ShouldNotHaveValidationError(long telegramId, long trainerId)
    {
        var dto = new UserExercisesByDateDto
        {
            TelegramId = telegramId,
            TrainerId = trainerId
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeTrue("because all required fields meet validation rules");
    }

    [Fact]
    public void Validate_WhenTelegramIdIsNegative_ShouldHaveValidationError()
    {
        var dto = new UserExercisesByDateDto
        {
            TelegramId = -1, 
            TrainerId = 0
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse("because Telegram ID must not be negative");
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Telegram ID must not be negative"));
    }

    [Fact]
    public void Validate_WhenTelegramIdIsEmpty_ShouldHaveValidationError()
    {
        var dto = new UserExercisesByDateDto
        {
            TelegramId = 0,
            TrainerId = 1
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse("because Telegram ID is required and must not be zero");
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Telegram ID is required"));
    }

    [Fact]
    public void Validate_WhenTrainerIdIsNegative_ShouldHaveValidationError()
    {
        var dto = new UserExercisesByDateDto
        {
            TelegramId = 1,
            TrainerId = -1 
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse("because Trainer ID must not be negative");
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Trainer ID must not be negative"));
    }
}