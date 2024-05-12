using FluentValidation;
using NutriFitLogBackend.Domain.DTOs.Nutrition.RequestDTOs;

namespace NutriFitLogBackend.Controllers.Nutrition.Validators;

public class DeleteUserFoodDtoValidator : AbstractValidator<DeleteUserFoodDto>
{
    public DeleteUserFoodDtoValidator()
    {
        RuleFor(x => x.TelegramId).NotEmpty().WithMessage("Telegram ID is required.");
        RuleFor(x => x.TelegramId).GreaterThan(0).WithMessage("Telegram ID must be positive.");
        RuleFor(x => x.MealId).NotEmpty().WithMessage("Meal ID is required.");
        RuleFor(x => x.MealId).GreaterThan(0).WithMessage("Meal ID must be positive.");
        RuleFor(x => x.FoodId).NotEmpty().WithMessage("Food ID is required.");
        RuleFor(x => x.FoodId).GreaterThan(0).WithMessage("Food ID must be positive.");
        RuleFor(x => x.DayPartId).NotEmpty().WithMessage("DayPart ID is required.");
        RuleFor(x => x.DayPartId).GreaterThan(0).WithMessage("Day Part ID must be positive.");
        RuleFor(x => x.TrainerId).GreaterThanOrEqualTo(0).WithMessage("Trainer ID must be positive.");
    }
}