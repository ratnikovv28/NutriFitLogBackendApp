using AutoFixture.Xunit2;
using FluentAssertions;
using NutriFitLogBackend.Controllers.Trainings.Validators;
using NutriFitLogBackend.Domain.DTOs.Trainings;
using Xunit;

namespace NutriFitLogBackend.Tests.API.Controllers.Trainings.Validators;

public class SetDtoValidatorTests
{
    private readonly SetDtoValidator _validator = new();

    [Theory, AutoData]
    public void Validate_WhenIdIsValid_ShouldNotHaveValidationError(long setId)
    {
        var dto = new SetDto { Id = setId };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenIdIsNegative_ShouldHaveValidationError()
    {
        var dto = new SetDto { Id = -1 };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Set ID must not be negative"));
    }
}