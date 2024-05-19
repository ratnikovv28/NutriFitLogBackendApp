using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NutriFitLogBackend.Controllers.Nutrition;
using NutriFitLogBackend.Domain.DTOs.Nutrition;
using NutriFitLogBackend.Domain.DTOs.Nutrition.RequestDTOs;
using NutriFitLogBackend.Domain.Extensions;
using NutriFitLogBackend.Domain.Services.Nutrition;
using Xunit;

namespace NutriFitLogBackend.Tests.API.Controllers.Nutrition;

public class NutritionControllerTests
{
    private readonly Mock<INutritionService> _nutritionServiceMock;
    private readonly NutritionController _controller;
    private readonly IFixture _fixture;

    public NutritionControllerTests()
    {
        _nutritionServiceMock = new Mock<INutritionService>();
        _controller = new NutritionController(_nutritionServiceMock.Object);
        _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
    }

    [Fact]
    public async Task GetAllDayParts_WhenCalled_ReturnsOkObjectResultWithDayParts()
    {
        // Arrange
        var dayParts = _fixture.CreateMany<DayPartDto>().ToList();
        _nutritionServiceMock.Setup(s => s.GetAllDayPartsAsync()).ReturnsAsync(dayParts);

        // Act
        var result = await _controller.GetAllDayParts();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(dayParts);
    }

    [Fact]
    public async Task GetAllFoods_WhenCalled_ReturnsOkObjectResultWithFoods()
    {
        // Arrange
        var foods = _fixture.CreateMany<FoodDto>().ToList();
        _nutritionServiceMock.Setup(s => s.GetAllFoodsAsync()).ReturnsAsync(foods);

        // Act
        var result = await _controller.GetAllFoods();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(foods);
    }

    [Fact]
    public async Task GetAvailableUserFoods_WithValidDto_ReturnsOkObjectResultWithAvailableUserFoods()
    {
        // Arrange
        var dto = _fixture.Create<AvailableUserFoodDto>();
        var availableFoods = _fixture.CreateMany<FoodDto>().ToList();
        _nutritionServiceMock.Setup(s => s.GetAvailableUserFoodAsync(dto.TelegramId, dto.MealId, dto.DayPartId, dto.TrainerId)).ReturnsAsync(availableFoods);

        // Act
        var result = await _controller.GetAvailableUserFoods(dto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(availableFoods);
    }

    [Fact]
    public async Task GetUserMealByDate_WithValidDto_ReturnsOkObjectResultWithUserMeal()
    {
        // Arrange
        var telegramId = _fixture.Create<long>();
        var trainerId = _fixture.Create<long>();
        var date = _fixture.Create<DateTime>().ToDateOnly();
        var dto = new UserFoodsByDateDto
        {
            TelegramId = telegramId,
            Date = date,
            TrainerId = trainerId
        };
        var meal = _fixture.Create<MealDto>();
        _nutritionServiceMock.Setup(s => s.GetUserMealByDateAsync(dto.TelegramId, dto.Date, dto.TrainerId)).ReturnsAsync(meal);

        // Act
        var result = await _controller.GetUserMealByDate(dto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(meal);
    }

    [Theory, AutoData]
    public async Task AddUserFood_WithValidDto_ReturnsOkResult(RequestFoodDto dto)
    {
        // Arrange
        _nutritionServiceMock.Setup(s => s.AddFoodAsync(dto)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddUserFood(dto);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Theory, AutoData]
    public async Task UpdateFoodMeal_WithValidDto_ReturnsOkResult(RequestFoodDto dto)
    {
        // Arrange
        _nutritionServiceMock.Setup(s => s.UpdateFoodMealAsync(dto)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateFoodMeal(dto);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Theory, AutoData]
    public async Task DeleteFood_WithValidDto_ReturnsOkResult(DeleteUserFoodDto dto)
    {
        // Arrange
        _nutritionServiceMock.Setup(s => s.DeleteFoodAsync(dto.TelegramId, dto.MealId, dto.FoodId, dto.DayPartId, dto.TrainerId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteFood(dto);

        // Assert
        result.Should().BeOfType<OkResult>();
    }
}