using AutoFixture.Xunit2;
using FluentAssertions;
using NutriFitLogBackend.Controllers.Nutrition.Validators;
using NutriFitLogBackend.Domain.DTOs.Nutrition.RequestDTOs;
using NutriFitLogBackend.Domain.Extensions;
using Xunit;

namespace NutriFitLogBackend.Tests.API.Controllers.Nutrition.Validators;

public class UserFoodsByDateDtoValidatorTests
{
    private readonly UserFoodsByDateDtoValidator _validator = new();

    [Theory, AutoData]
    public void Validate_WhenAllFieldsAreValid_ShouldNotHaveValidationError(long telegramId, long trainerId, DateTime date)
    {
        var dto = new UserFoodsByDateDto
        {
            TelegramId = telegramId,
            TrainerId = trainerId,
            Date = date.ToDateOnly()
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }

    [Theory, AutoData]
    public void Validate_WhenTelegramIdIsZero_ShouldHaveValidationError(long trainerId, DateTime date)
    {
        var dto = new UserFoodsByDateDto
        {
            TelegramId = 0,  // Invalid TelegramId
            TrainerId = trainerId,
            Date = date.ToDateOnly()
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Telegram ID is required"));
    }

    [Theory, AutoData]
    public void Validate_WhenTrainerIdIsNegative_ShouldHaveValidationError(long telegramId, DateTime date)
    {
        var dto = new UserFoodsByDateDto
        {
            TelegramId = telegramId,
            TrainerId = -1,  // Invalid TrainerId
            Date = date.ToDateOnly()
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Trainer ID must be positive"));
    }

    [Fact]
    public void Validate_WhenDateIsEmpty_ShouldHaveValidationError()
    {
        var dto = new UserFoodsByDateDto
        {
            TelegramId = 1,
            TrainerId = 1,
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Date is required"));
    }
}