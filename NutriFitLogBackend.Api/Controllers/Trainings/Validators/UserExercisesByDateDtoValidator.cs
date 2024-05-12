using FluentValidation;
using NutriFitLogBackend.Domain.DTOs.Trainings.RequestDTOs;

namespace NutriFitLogBackend.Controllers.Trainings.Validators;

public class UserExercisesByDateDtoValidator : AbstractValidator<UserExercisesByDateDto>
{
    public UserExercisesByDateDtoValidator()
    {
        RuleFor(x => x.TelegramId).NotEmpty().WithMessage("Telegram ID is required.");
        RuleFor(x => x.TelegramId).GreaterThan(0).WithMessage("Telegram ID must not be negative.");
        RuleFor(x => x.TrainerId).GreaterThanOrEqualTo(0).WithMessage("Trainer ID must not be negative.");
    }
}
