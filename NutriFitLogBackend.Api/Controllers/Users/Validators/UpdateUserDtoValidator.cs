using FluentValidation;
using NutriFitLogBackend.Domain.DTOs.Users;

namespace NutriFitLogBackend.Controllers.Users.Validators;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.TelegramId).NotEmpty().WithMessage("Telegram ID is required.");
    }
}