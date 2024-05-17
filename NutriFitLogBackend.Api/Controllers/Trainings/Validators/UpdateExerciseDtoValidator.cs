using FluentValidation;
using NutriFitLogBackend.Domain.DTOs.Trainings.RequestDTOs;

namespace NutriFitLogBackend.Controllers.Trainings.Validators;

public class UpdateExerciseDtoValidator : AbstractValidator<UpdateExerciseDto>
{
    public UpdateExerciseDtoValidator()
    {
        RuleFor(x => x.TelegramId).NotEmpty().WithMessage("Telegram ID is required.");
        RuleFor(x => x.TelegramId).GreaterThan(0).WithMessage("Telegram ID must not be negative.");
        RuleFor(x => x.TrainingId).NotEmpty().WithMessage("Training ID is required.");
        RuleFor(x => x.TrainingId).GreaterThan(0).WithMessage("Training ID must not be negative.");
        RuleFor(x => x.ExerciseId).NotEmpty().WithMessage("Exercise ID is required.");
        RuleFor(x => x.ExerciseId).GreaterThan(0).WithMessage("Exercise ID must not be negative.");
        RuleFor(x => x.TrainerId).GreaterThanOrEqualTo(0).WithMessage("Trainer ID must not be negative.");
        RuleFor(x => x.Sets).NotEmpty().WithMessage("Sets is required.");
        RuleFor(x => x.Sets.Count).GreaterThan(0).WithMessage("Sets count must be positive.");
        RuleForEach(x => x.Sets).SetValidator(new SetDtoValidator());
    }
}