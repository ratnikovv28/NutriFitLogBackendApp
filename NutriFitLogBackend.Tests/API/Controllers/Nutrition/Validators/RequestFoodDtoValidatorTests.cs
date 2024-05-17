using AutoFixture.Xunit2;
using FluentAssertions;
using NutriFitLogBackend.Controllers.Nutrition.Validators;
using NutriFitLogBackend.Domain.DTOs.Nutrition;
using Xunit;

namespace NutriFitLogBackend.Tests.API.Controllers.Nutrition.Validators;

public class RequestFoodDtoValidatorTests
{
    private readonly RequestFoodDtoValidator _validator = new();

    [Theory, AutoData]
    public void Validate_WhenAllFieldsAreValid_ShouldNotHaveValidationError(
        long telegramId, long mealId, long foodId, long dayPartId, long trainerId, 
        double quantity, double? calories, double? protein, double? fats, double? carbohydrates)
    {
        var dto = new RequestFoodDto
        {
            TelegramId = telegramId,
            MealId = mealId,
            FoodId = foodId,
            DayPartId = dayPartId,
            TrainerId = trainerId,
            Quantity = quantity,
            Calories = calories,
            Protein = protein,
            Fats = fats,
            Carbohydrates = carbohydrates
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }

    [Theory, AutoData]
    public void Validate_WhenTelegramIdIsZero_ShouldHaveValidationError(long mealId, long foodId, long dayPartId, long trainerId, double quantity)
    {
        var dto = new RequestFoodDto
        {
            TelegramId = 0,
            MealId = mealId,
            FoodId = foodId,
            DayPartId = dayPartId,
            TrainerId = trainerId,
            Quantity = quantity
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Telegram ID is required"));
    }

    [Theory, AutoData]
    public void Validate_WhenNegativeValuesPresent_ShouldHaveValidationError(
        long telegramId, long mealId, long foodId, long dayPartId, long trainerId)
    {
        var dto = new RequestFoodDto
        {
            TelegramId = telegramId,
            MealId = mealId,
            FoodId = foodId,
            DayPartId = dayPartId,
            TrainerId = trainerId,
            Quantity = -1,
            Calories = -1,
            Protein = -1,
            Fats = -1,
            Carbohydrates = -1
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterOrEqualTo(5); // Checking for all negative field validations
    }

    [Theory, AutoData]
    public void Validate_WhenOptionalFieldsAreNull_ShouldStillPass(
        long telegramId, long mealId, long foodId, long dayPartId, long trainerId, double quantity)
    {
        var dto = new RequestFoodDto
        {
            TelegramId = telegramId,
            MealId = mealId,
            FoodId = foodId,
            DayPartId = dayPartId,
            TrainerId = trainerId,
            Quantity = quantity,
            Calories = null,
            Protein = null,
            Fats = null,
            Carbohydrates = null
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeTrue(); // Optional fields being null should not affect validity
    }
}