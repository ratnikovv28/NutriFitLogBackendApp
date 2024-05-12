using FluentValidation;
using NutriFitLogBackend.Domain.DTOs.Trainings.RequestDTOs;

namespace NutriFitLogBackend.Controllers.Trainings.Validators;

public class DeleteSetsExerciseDtoValidator : AbstractValidator<DeleteSetsExerciseDto>
{
    public DeleteSetsExerciseDtoValidator()
    {
        RuleFor(x => x.TelegramId).NotEmpty().WithMessage("Telegram ID is required.");
        RuleFor(x => x.TelegramId).GreaterThan(0).WithMessage("Telegram ID must not be negative.");
        RuleFor(x => x.TrainingId).NotEmpty().WithMessage("Training ID is required.");
        RuleFor(x => x.TrainingId).GreaterThan(0).WithMessage("Training ID must not be negative.");
        RuleFor(x => x.ExerciseId).NotEmpty().WithMessage("Exercise ID is required.");
        RuleFor(x => x.ExerciseId).GreaterThan(0).WithMessage("Exercise ID must not be negative.");
        RuleFor(x => x.TrainerId).GreaterThanOrEqualTo(0).WithMessage("Trainer ID must not be negative.");
        RuleFor(x => x.SetId).NotEmpty().WithMessage("Set ID is required.");
        RuleFor(x => x.SetId).GreaterThan(0).WithMessage("Set ID must not be negative.");
    }
}