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
    public async Task GetAllDayParts_ReturnsOkObjectResult_WithDayParts()
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
    public async Task GetAllFoods_ReturnsOkObjectResult_WithFoods()
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
    public async Task GetUserFoodsByDate_ReturnsOkObjectResult_WithUserFoods()
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
        
        var userFoods = _fixture.CreateMany<MealFoodDto>().ToList();
        _nutritionServiceMock.Setup(s => s.GetUserFoodsByDateAsync(dto.TelegramId, dto.Date, dto.TrainerId)).ReturnsAsync(userFoods);

        // Act
        var result = await _controller.GetUserFoodsByDate(dto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(userFoods);
    }

    [Theory, AutoData]
    public async Task AddUserFood_ReturnsOkResult(RequestFoodDto dto)
    {
        // Arrange
        _nutritionServiceMock.Setup(s => s.AddFoodAsync(dto)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddUserFood(dto);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Theory, AutoData]
    public async Task UpdateFoodMeal_ReturnsOkResult(RequestFoodDto dto)
    {
        // Arrange
        _nutritionServiceMock.Setup(s => s.UpdateFoodMealAsync(dto)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateFoodMeal(dto);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Theory, AutoData]
    public async Task DeleteFood_ReturnsOkResult(DeleteUserFoodDto dto)
    {
        // Arrange
        _nutritionServiceMock.Setup(s => s.DeleteFoodAsync(dto.TelegramId, dto.MealId, dto.FoodId, dto.DayPartId, dto.TrainerId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteFood(dto);

        // Assert
        result.Should().BeOfType<OkResult>();
    }
}