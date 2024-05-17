using AutoFixture.Xunit2;
using FluentAssertions;
using NutriFitLogBackend.Controllers.Users.Validators;
using NutriFitLogBackend.Domain.DTOs.Users;
using NutriFitLogBackend.Domain.DTOs.Users.RequestDTOs;
using Xunit;

namespace NutriFitLogBackend.Tests.API.Controllers.Users;

public class CreateUserDtoValidatorTests
{
    private readonly CreateUserDtoValidator _validator = new();

    [Theory, AutoData]
    public void Validate_WhenDataOk_ShouldNotHaveValidationError(long telegramId)
    {
        var request = new CreateUserDto
        {
            TelegramId = telegramId
        };

        var result = _validator.Validate(request);
        result.IsValid.Should().BeTrue();
    }
    
    [Theory, AutoData]
    public void Validate_WhenTelegramIdNotPositive_ShouldHaveValidationError(long telegramId)
    {
        var request = new CreateUserDto
        {
            TelegramId = -telegramId
        };

        var result = _validator.Validate(request);
        result.IsValid.Should().BeFalse();
    }
}