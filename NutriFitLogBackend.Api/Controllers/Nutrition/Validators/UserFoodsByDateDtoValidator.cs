using FluentValidation;
using NutriFitLogBackend.Domain.DTOs.Nutrition.RequestDTOs;

namespace NutriFitLogBackend.Controllers.Nutrition.Validators;

public class UserFoodsByDateDtoValidator : AbstractValidator<UserFoodsByDateDto>
{
    public UserFoodsByDateDtoValidator()
    {
        RuleFor(x => x.TelegramId).NotEmpty().WithMessage("Telegram ID is required.");
        RuleFor(x => x.TelegramId).GreaterThan(0).WithMessage("Telegram ID must be positive.");
        RuleFor(x => x.TrainerId).GreaterThanOrEqualTo(0).WithMessage("Trainer ID must be positive.");
        RuleFor(x => x.Date).NotEmpty().WithMessage("Date is required.");
    }
}