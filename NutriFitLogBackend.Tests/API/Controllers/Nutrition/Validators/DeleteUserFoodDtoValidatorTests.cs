using AutoFixture.Xunit2;
using FluentAssertions;
using NutriFitLogBackend.Controllers.Nutrition.Validators;
using NutriFitLogBackend.Domain.DTOs.Nutrition.RequestDTOs;
using Xunit;

namespace NutriFitLogBackend.Tests.API.Controllers.Nutrition.Validators;

public class DeleteUserFoodDtoValidatorTests
{
    private readonly DeleteUserFoodDtoValidator _validator = new();

    [Theory, AutoData]
    public void Validate_WhenAllFieldsAreValid_ShouldNotHaveValidationError(long telegramId, long mealId, long foodId, long dayPartId, long trainerId)
    {
        var dto = new DeleteUserFoodDto
        {
            TelegramId = telegramId,
            MealId = mealId,
            FoodId = foodId,
            DayPartId = dayPartId,
            TrainerId = trainerId
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenTelegramIdIsNegative_ShouldHaveValidationError()
    {
        var dto = new DeleteUserFoodDto
        {
            TelegramId = -1,
            MealId = 1,
            FoodId = 1,
            DayPartId = 1,
            TrainerId = 0
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Telegram ID must be positive"));
    }

    [Theory, AutoData]
    public void Validate_WhenMealIdIsZero_ShouldHaveValidationError(long telegramId, long trainerId, long foodId, long dayPartId)
    {
        var dto = new DeleteUserFoodDto
        {
            TelegramId = telegramId,
            MealId = 0,
            FoodId = foodId,
            DayPartId = dayPartId,
            TrainerId = trainerId
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Meal ID must be positive"));
    }

    [Theory, AutoData]
    public void Validate_WhenFoodIdIsNegative_ShouldHaveValidationError(long telegramId, long mealId, long dayPartId, long trainerId)
    {
        var dto = new DeleteUserFoodDto
        {
            TelegramId = telegramId,
            MealId = mealId,
            FoodId = -1,
            DayPartId = dayPartId,
            TrainerId = trainerId
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Food ID must be positive"));
    }

    [Theory, AutoData]
    public void Validate_WhenDayPartIdIsNegative_ShouldHaveValidationError(long telegramId, long mealId, long foodId, long trainerId)
    {
        var dto = new DeleteUserFoodDto
        {
            TelegramId = telegramId,
            MealId = mealId,
            FoodId = foodId,
            DayPartId = -1,
            TrainerId = trainerId
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Day Part ID must be positive"));
    }

    [Theory, AutoData]
    public void Validate_WhenTrainerIdIsNegative_ShouldHaveValidationError(long telegramId, long mealId, long foodId, long dayPartId)
    {
        var dto = new DeleteUserFoodDto
        {
            TelegramId = telegramId,
            MealId = mealId,
            FoodId = foodId,
            DayPartId = dayPartId,
            TrainerId = -1
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Trainer ID must be positive"));
    }
}