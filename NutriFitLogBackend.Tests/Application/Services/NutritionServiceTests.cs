using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using FluentAssertions;
using Moq;
using AutoFixture.Xunit2;
using NutriFitLogBackend.Application.Services.Nutrition;
using NutriFitLogBackend.Domain;
using NutriFitLogBackend.Domain.DTOs.Nutrition;
using NutriFitLogBackend.Domain.Entities.Nutrition;
using NutriFitLogBackend.Domain.Entities.Users;
using NutriFitLogBackend.Domain.Exceptions;
using NutriFitLogBackend.Domain.Extensions;
using NutriFitLogBackend.Infrastructure.Mapper;
using Xunit;

public class NutritionServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly IMapper _mapper;
    private readonly NutritionService _sut;
    private readonly IFixture _fixture;

    public NutritionServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = new Mapper(config);
        _sut = new NutritionService(_uowMock.Object, _mapper);
        _fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
    }

    [Fact]
    public async Task GetAllDayPartsAsync_ReturnsAllDayParts()
    {
        // Arrange
        var dayPartName = _fixture.Create<string>();
        var daypart = new DayPart
        {
            Name = dayPartName
        };
        _uowMock.Setup(u => u.DayPartRepository.GetAllAsync()).ReturnsAsync(new []{daypart});

        // Act
        var result = await _sut.GetAllDayPartsAsync();

        // Assert
        result.Should().BeEquivalentTo(_mapper.Map<IReadOnlyCollection<DayPartDto>>(new []{daypart}));
    }

    [Theory]
    [MemberData(nameof(FoodData))]
    public async Task GetAllFoodsAsync_ReturnsAllFoods(RequestFoodDto dto, long telegramId, Food food)
    {
        // Arrange
        _uowMock.Setup(u => u.FoodRepository.GetAllAsync()).ReturnsAsync(new []{food});

        // Act
        var result = await _sut.GetAllFoodsAsync();

        // Assert
        result.Should().BeEquivalentTo(_mapper.Map<IReadOnlyCollection<FoodDto>>(new []{food}));
    }

    [Theory, AutoData]
    public async Task GetAvailableUserFoodAsync_WhenUserDoesNotExist_ThrowsUserNotFoundException(long telegramId, long mealId, long dayPartId, long trainerId)
    {
        // Arrange
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.GetAvailableUserFoodAsync(telegramId, mealId, dayPartId, trainerId);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Theory, AutoData]
    public async Task GetAvailableUserFoodAsync_WhenUserDoesNotWorkWithTrainer_ThrowsStudentTrainerWorkException(long telegramId, long mealId, long dayPartId, long trainerId)
    {
        // Arrange
        var user = new User(telegramId);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync(user);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(trainerId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.GetAvailableUserFoodAsync(user.TelegramId, mealId, dayPartId, trainerId);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Theory]
    [MemberData(nameof(AvailableFoodData))]
    public async Task GetAvailableUserFoodAsync_ReturnsAvailableFoods(User user, Meal meal, List<Food> foods, long dayPartId)
    {
        // Arrange
        var mealId = meal.Id;
        meal.Foods = foods.Select(f => new MealFood { FoodId = f.Id + 1, DayPartId = dayPartId }).ToList();
        user.Meals.Add(meal);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(user.TelegramId)).ReturnsAsync(user);
        _uowMock.Setup(u => u.FoodRepository.GetAllAsync()).ReturnsAsync(foods);

        // Act
        var result = await _sut.GetAvailableUserFoodAsync(user.TelegramId, mealId, dayPartId);

        // Assert
        result.Should().BeEquivalentTo(_mapper.Map<IReadOnlyCollection<FoodDto>>(foods));
    }

    [Theory, AutoData]
    public async Task GetUserMealByDateAsync_WhenUserDoesNotExist_ThrowsUserNotFoundException(long telegramId, DateTime date, long trainerId)
    {
        // Arrange
        var dateOnly = date.ToDateOnly();
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.GetUserMealByDateAsync(telegramId, dateOnly, trainerId);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Theory, AutoData]
    public async Task GetUserMealByDateAsync_ReturnsMeal(long telegramId, DateTime date)
    {
        // Arrange
        var user = new User(telegramId);
        var meal = new Meal { CreatedDate = date };
        user.Meals.Add(meal);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(user.TelegramId)).ReturnsAsync(user);

        // Act
        var result = await _sut.GetUserMealByDateAsync(user.TelegramId, date.ToDateOnly());

        // Assert
        result.Should().BeEquivalentTo(_mapper.Map<MealDto>(meal));
    }
    
    [Theory, AutoData]
    public async Task GetUserMealByDateAsync_WhemMealsDoesNotExist_ReturnsMeal(long telegramId, DateTime date)
    {
        // Arrange
        var user = new User(telegramId);
        var meal = new Meal { CreatedDate = date.AddDays(1) };
        var expectedMeal = new Meal { CreatedDate = date };
        user.Meals.Add(meal);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(user.TelegramId)).ReturnsAsync(user);
        _uowMock.Setup(u => u.NutritionRepository.AddAsync(It.IsAny<Meal>())).ReturnsAsync(expectedMeal);

        // Act
        var result = await _sut.GetUserMealByDateAsync(user.TelegramId, date.ToDateOnly());

        // Assert
        result.Should().BeEquivalentTo(_mapper.Map<MealDto>(expectedMeal));
    }

    [Theory, AutoData]
    public async Task AddFoodAsync_WhenUserDoesNotExist_ThrowsUserNotFoundException(RequestFoodDto dto)
    {
        // Arrange
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(dto.TelegramId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.AddFoodAsync(dto);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Theory]
    [MemberData(nameof(FoodData))]
    public async Task AddFoodAsync_AddsFood(RequestFoodDto dto, long telegramId, Food food)
    {
        // Arrange
        var user = new User(telegramId)
        {
            Id = telegramId + 1
        };
        var meal = new Meal { Id = dto.MealId, UserId = user.Id, Foods = new List<MealFood>()
        {
            new MealFood()
            {
                DayPartId = dto.DayPartId + 1,
                FoodId = dto.FoodId + 1,
                MealId = dto.MealId
            }
        }};
        var trainer = new User(dto.TrainerId)
        {
            Id = dto.TrainerId + 1
        };
        var studentTrainer = new StudentTrainer()
        {
            Id = telegramId + 1,
            IsWorking = true,
            StudentId = telegramId,
            Student = user,
            TrainerId = dto.TrainerId,
            Trainer = trainer
        };
        user.Trainers.Add(studentTrainer);
        user.Meals.Add(meal);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(dto.TelegramId)).ReturnsAsync(user);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(dto.TrainerId)).ReturnsAsync(trainer);
        _uowMock.Setup(u => u.StudentTrainerRepository.GetRelationShip(telegramId + 1,dto.TrainerId + 1)).ReturnsAsync(studentTrainer);
        _uowMock.Setup(u => u.FoodRepository.ExistAsync(dto.FoodId)).ReturnsAsync(true);
        _uowMock.Setup(u => u.MealFoodRepository.AddAsync(It.IsAny<MealFood>())).ReturnsAsync(new MealFood());

        // Act
        await _sut.AddFoodAsync(dto);

        // Assert
        _uowMock.Verify(u => u.MealFoodRepository.AddAsync(It.Is<MealFood>(mf => mf.FoodId == dto.FoodId && mf.MealId == dto.MealId && mf.DayPartId == dto.DayPartId)), Times.Once);
        _uowMock.Verify(u => u.SaveAsync(), Times.Once);
    }
    
    [Theory]
    [MemberData(nameof(FoodData))]
    public async Task AddFoodAsync_WhenFoodAlreadyExist_ShouldThrows(RequestFoodDto dto, long telegramId, Food food)
    {
        // Arrange
        var user = new User(telegramId)
        {
            Id = telegramId + 1
        };
        var meal = new Meal { Id = dto.MealId, UserId = user.Id, Foods = new List<MealFood>()
        {
            new MealFood()
            {
                DayPartId = dto.DayPartId,
                FoodId = dto.FoodId,
                MealId = dto.MealId
            }
        }};
        var trainer = new User(dto.TrainerId)
        {
            Id = dto.TrainerId + 1
        };
        var studentTrainer = new StudentTrainer()
        {
            Id = telegramId + 1,
            IsWorking = true,
            StudentId = telegramId,
            Student = user,
            TrainerId = dto.TrainerId,
            Trainer = trainer
        };
        user.Trainers.Add(studentTrainer);
        user.Meals.Add(meal);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(dto.TelegramId)).ReturnsAsync(user);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(dto.TrainerId)).ReturnsAsync(trainer);
        _uowMock.Setup(u => u.StudentTrainerRepository.GetRelationShip(telegramId + 1,dto.TrainerId + 1)).ReturnsAsync(studentTrainer);
        _uowMock.Setup(u => u.FoodRepository.ExistAsync(dto.FoodId)).ReturnsAsync(true);
        _uowMock.Setup(u => u.MealFoodRepository.AddAsync(It.IsAny<MealFood>())).ReturnsAsync(new MealFood());

        // Act
        var act = () => _sut.AddFoodAsync(dto);

        // Assert
        await act.Should().ThrowAsync<FoodExistsInMealDayPartException>();
    }

    [Theory, AutoData]
    public async Task UpdateFoodMealAsync_WhenUserDoesNotExist_ThrowsUserNotFoundException(RequestFoodDto dto)
    {
        // Arrange
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(dto.TelegramId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.UpdateFoodMealAsync(dto);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Theory]
    [MemberData(nameof(MealsData))]
    public async Task UpdateFoodMealAsync_UpdatesFood(RequestFoodDto dto, User user, MealFood mealFood)
    {
        // Arrange
        user.Id = dto.TelegramId + 1;
        var trainer = new User(dto.TrainerId)
        {
            Id = dto.TrainerId + 1
        };
        var studentTrainer = new StudentTrainer()
        {
            Id = dto.TelegramId + 1,
            IsWorking = true,
            StudentId = dto.TelegramId,
            Student = user,
            TrainerId = dto.TrainerId,
            Trainer = trainer
        };
        user.Trainers.Add(studentTrainer);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(dto.TelegramId)).ReturnsAsync(user);
        _uowMock.Setup(u => u.StudentTrainerRepository.GetRelationShip(dto.TelegramId + 1,dto.TrainerId + 1)).ReturnsAsync(studentTrainer);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(dto.TrainerId)).ReturnsAsync(trainer);
        _uowMock.Setup(u => u.FoodRepository.ExistAsync(dto.FoodId)).ReturnsAsync(true);
        _uowMock.Setup(u => u.MealFoodRepository.GetById(mealFood.Id)).ReturnsAsync(mealFood);

        // Act
        await _sut.UpdateFoodMealAsync(dto);

        // Assert
        _uowMock.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Theory, AutoData]
    public async Task DeleteFoodAsync_WhenUserDoesNotExist_ThrowsUserNotFoundException(long telegramId, long mealId, long foodId, long dayPartId, long trainerId)
    {
        // Arrange
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _sut.DeleteFoodAsync(telegramId, mealId, foodId, dayPartId, trainerId);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Theory]
    [MemberData(nameof(DeleteFoodData))]
    public async Task DeleteFoodAsync_DeletesFood(long telegramId, long mealId, long foodId, long dayPartId, long trainerId, User user, MealFood mealFood)
    {
        // Arrange
        user.Id = telegramId + 1;
        var trainer = new User(trainerId){
            Id = trainerId + 1
        };
        var studentTrainer = new StudentTrainer()
        {
            Id = telegramId + 1,
            IsWorking = true,
            StudentId = telegramId,
            Student = user,
            TrainerId = trainerId,
            Trainer = trainer
        };
        user.Trainers.Add(studentTrainer);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(telegramId)).ReturnsAsync(user);
        _uowMock.Setup(u => u.UserRepository.GetByTelegramIdAsync(trainerId)).ReturnsAsync(trainer);
        _uowMock.Setup(u => u.StudentTrainerRepository.GetRelationShip(telegramId + 1,trainerId + 1)).ReturnsAsync(studentTrainer);
        _uowMock.Setup(u => u.FoodRepository.ExistAsync(foodId)).ReturnsAsync(true);
        _uowMock.Setup(u => u.MealFoodRepository.GetById(mealFood.Id)).ReturnsAsync(mealFood);

        // Act
        await _sut.DeleteFoodAsync(telegramId, mealId, foodId, dayPartId, trainerId);

        // Assert
        _uowMock.Verify(u => u.MealFoodRepository.Delete(It.Is<MealFood>(mf => mf.Id == mealFood.Id)), Times.Once);
        _uowMock.Verify(u => u.SaveAsync(), Times.Once);
    }
    
    [ExcludeFromCodeCoverage]
    public static IEnumerable<object[]> FoodData()
    {
        var fixture = new Fixture();
        var telegramId = fixture.Create<long>();
        var requestFoodDto = fixture.Create<RequestFoodDto>();
        var food = new Food
        {
            Description = fixture.Create<string>(),
            Name = fixture.Create<string>(),
            PictureUrl = fixture.Create<string>(),
            Unit = UnitOfMeasure.Grams
        };

        yield return new object[]
        {
            requestFoodDto,
            telegramId,
            food
        };
    }
    
    [ExcludeFromCodeCoverage]
    public static IEnumerable<object[]> AvailableFoodData()
    {
        var fixture = new Fixture();
        var telegramId = fixture.Create<long>();
        var user = new User(telegramId);
        var dayPartId = fixture.Create<long>();
        var food = new Food
        {
            Id = fixture.Create<long>(),
            Description = fixture.Create<string>(),
            Name = fixture.Create<string>(),
            PictureUrl = fixture.Create<string>(),
            Unit = UnitOfMeasure.Grams
        };

        var meal = new Meal
        {
            Id = fixture.Create<long>(),
            CreatedDate = DateTime.UtcNow.AddDays(-1),
            UserId = fixture.Create<long>(),
            Foods = new List<MealFood>
            {
                new MealFood
                {
                    Id = fixture.Create<long>(),
                    Food = new Food()
                    {
                        Id = fixture.Create<long>(),
                        Name = fixture.Create<string>(),
                    },
                    Quantity = 100 + 10 * 1,
                }
            }
        };

        var foods = new List<Food>()
        {
            food
        };
        
        yield return new object[]
        {
            user,
            meal,
            foods,
            dayPartId
        };
    }
    
    [ExcludeFromCodeCoverage]
    public static IEnumerable<object[]> MealsData()
    {
        var fixture = new Fixture();
        var requestDto = fixture.Create<RequestFoodDto>();
        var user = new User(requestDto.TelegramId);
        var food = new Food()
        {
            Id = requestDto.FoodId,
            Name = "Food1"
        };
        
        var foods = new MealFood
        {
            Food = food,
            Quantity = 100 + 10 * 1,
            DayPartId = requestDto.DayPartId,
            FoodId = requestDto.FoodId
        };
        
        var meal = new Meal
        {
            Id = requestDto.MealId,
            CreatedDate = DateTime.UtcNow.AddDays(-1),
            UserId = fixture.Create<long>(),
            Foods = new List<MealFood>
            {
                foods
            }
        };
        
        user.Meals.Add(meal);
        
        yield return new object[] { requestDto, user, foods };
    }
    
    [ExcludeFromCodeCoverage]
    public static IEnumerable<object[]> DeleteFoodData()
    {
        var fixture = new Fixture();
        var requestDto = fixture.Create<RequestFoodDto>();
        var user = new User(requestDto.TelegramId);
        var food = new Food()
        {
            Id = requestDto.FoodId,
            Name = "Food1"
        };
        
        var foods = new MealFood
        {
            Food = food,
            Quantity = 100 + 10 * 1,
            DayPartId = requestDto.DayPartId,
            FoodId = requestDto.FoodId
        };
        
        var meal = new Meal
        {
            Id = requestDto.MealId,
            CreatedDate = DateTime.UtcNow.AddDays(-1),
            UserId = fixture.Create<long>(),
            Foods = new List<MealFood>
            {
                foods,
            }
        };
        
        user.Meals.Add(meal);
        
        yield return new object[] { 
            requestDto.TelegramId,
            requestDto.MealId,
            requestDto.FoodId,
            requestDto.DayPartId,
            requestDto.TrainerId,
            user,
            foods };
    }
}