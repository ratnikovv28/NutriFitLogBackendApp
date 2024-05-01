using FluentValidation;
using NutriFitLogBackend.Domain.DTOs.Users;

namespace NutriFitLogBackend.Controllers.Users.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.TelegramId).NotEmpty().WithMessage("Telegram ID is required.");
        RuleFor(x => x.TelegramId).GreaterThan(0).WithMessage("Telegram ID must not be negative.");
    }
}