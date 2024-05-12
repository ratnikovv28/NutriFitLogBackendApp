using FluentValidation;
using NutriFitLogBackend.Domain.DTOs.Nutrition;

namespace NutriFitLogBackend.Controllers.Nutrition.Validators;

public class RequestFoodDtoValidator : AbstractValidator<RequestFoodDto>
{
    public RequestFoodDtoValidator()
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
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be positive.");
        RuleFor(x => x.Calories).GreaterThanOrEqualTo(0).When(x => x.Calories.HasValue).WithMessage("Calories must not be negative.");
        RuleFor(x => x.Protein).GreaterThanOrEqualTo(0).When(x => x.Protein.HasValue).WithMessage("Protein must not be negative.");
        RuleFor(x => x.Fats).GreaterThanOrEqualTo(0).When(x => x.Fats.HasValue).WithMessage("Fats must not be negative.");
        RuleFor(x => x.Carbohydrates).GreaterThanOrEqualTo(0).When(x => x.Carbohydrates.HasValue).WithMessage("Carbohydrates must not be negative.");
    }
}