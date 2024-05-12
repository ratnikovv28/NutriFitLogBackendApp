using FluentValidation;
using NutriFitLogBackend.Domain.DTOs.Trainings;

namespace NutriFitLogBackend.Controllers.Trainings.Validators;

public class SetDtoValidator : AbstractValidator<SetDto>
{
    public SetDtoValidator()
    {
        RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage("Set ID must not be negative.");
    }
}