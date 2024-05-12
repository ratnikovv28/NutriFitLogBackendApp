using AutoMapper;
using NutriFitLogBackend.Domain;
using NutriFitLogBackend.Domain.DTOs.Nutrition;
using NutriFitLogBackend.Domain.Entities.Nutrition;
using NutriFitLogBackend.Domain.Entities.Users;
using NutriFitLogBackend.Domain.Exceptions;
using NutriFitLogBackend.Domain.Extensions;
using NutriFitLogBackend.Domain.Services.Nutrition;

namespace NutriFitLogBackend.Application.Services.Nutrition;

public class NutritionService : INutritionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public NutritionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // Получение всех частей дня
    public async Task<IReadOnlyCollection<DayPartDto>> GetAllDayPartsAsync()
    {
        var dayParts = await _unitOfWork.DayPartRepository.GetAllAsync();
        
        return _mapper.Map<IReadOnlyCollection<DayPartDto>>(dayParts);
    }
    
    // Получение всей ебы
    public async Task<IReadOnlyCollection<FoodDto>> GetAllFoodsAsync()
    {
        var foods = await _unitOfWork.FoodRepository.GetAllAsync();
        
        return _mapper.Map<IReadOnlyCollection<FoodDto>>(foods);
    }

    // Получение информации об еде 
    public async Task<IReadOnlyCollection<MealFoodDto>> GetUserFoodsByDateAsync(long telegramId, DateOnly date, long trainerId = 0)
    {
        // Получение пользователя
        var user = await GetUser(telegramId);
        
        // Если пользователь не работает с тренером, то ошибка
        UserWorkWithTrainerGuard(user, trainerId);
        
        // Приемы пищи по дню
        var mealByDate = user.Meals.FirstOrDefault(m => m.CreatedDate.ToDateOnly() == date);
        
        // Если в указанный день не было тренировки, то нужно создать
        if (mealByDate is null)
        {
            var meal = new Meal
            {
                CreatedDate = date.ToDateTimeUtc(),
                Foods = new List<MealFood>(),
                UserId = user.Id
            };
            mealByDate = await _unitOfWork.NutritionRepository.AddAsync(meal);
            await _unitOfWork.SaveAsync();
        }
        
        // Еда в нужный день
        var foodsByDate = mealByDate.Foods;
        
        return _mapper.Map<IReadOnlyCollection<MealFoodDto>>(foodsByDate);
    }
    
    // Добавление еды пользователю
    public async Task AddFoodAsync(RequestFoodDto dto)
    {
        // Получение пользователя
        var user = await GetUser(dto.TelegramId);

        // Проверки на валидность входных данных
        GetMealById(user, dto.MealId);
        UserWorkWithTrainerGuard(user, dto.TrainerId);
        await FoodExistsGuard(dto.FoodId);
        FoodExistsInMealWithDayPartGuard(user, dto.MealId, dto.FoodId, dto.DayPartId);

        var mealFood = new MealFood
        {
            MealId = dto.MealId,
            DayPartId = dto.DayPartId,
            FoodId = dto.FoodId,
            Calories = dto.Calories,
            Protein = dto.Protein,
            Fats = dto.Fats,
            Carbohydrates = dto.Carbohydrates,
            Quantity = dto.Quantity
        };

        await _unitOfWork.MealFoodRepository.AddAsync(mealFood);
        await _unitOfWork.SaveAsync();
    }
    
    // Обновление еды у приема пищи
    public async Task UpdateFoodMealAsync(RequestFoodDto dto)
    {
        // Получение пользователя
        var user = await GetUser(dto.TelegramId);

        UserWorkWithTrainerGuard(user, dto.TrainerId);

        var userMealFood = GetUserFoodByMeal(user, dto.MealId, dto.FoodId, dto.DayPartId);
        
        var updatedFood = new MealFood
        {
            Id = userMealFood.Id,
            MealId = dto.MealId,
            FoodId = dto.FoodId,
            Calories = dto.Calories,
            Protein = dto.Protein,
            Carbohydrates = dto.Carbohydrates,
            DayPartId = dto.DayPartId,
            Fats = dto.Fats,
            Quantity = dto.Quantity
        };
        
        //TODO доделать
        _unitOfWork.MealFoodRepository.Update(updatedFood);
        await _unitOfWork.SaveAsync();
    }
    
    // Удаление еды из приема пищи
    public async Task DeleteFoodAsync(long telegramId, long mealId, long foodId, long dayPartId, long trainerId = 0)
    {
        // Получение пользователя
        var user = await GetUser(telegramId);

        // Проверки на валидность входных данных
        UserWorkWithTrainerGuard(user, trainerId);

        var userMealFood = GetUserFoodByMeal(user, mealId, foodId, dayPartId);

        _unitOfWork.MealFoodRepository.Delete(userMealFood);
        await _unitOfWork.SaveAsync();
    }
    
    private async Task<User> GetUser(long telegramId)
    {
        var user = await _unitOfWork.UserRepository.GetByTelegramIdAsync(telegramId);
        if (user == null)
            throw new UserNotFoundException(telegramId);

        return user;
    }
    
    private void UserWorkWithTrainerGuard(User user, long trainerId)
    {
        if(trainerId != 0 && user.Trainers.All(u => u.TelegramId != trainerId))
            throw new NoAccessUserDataException(user.TelegramId, trainerId);
    }
    
    private Meal GetMealById(User user, long mealId)
    {
        var userMealById = user.Meals.SingleOrDefault(m => m.Id == mealId) 
                               ?? throw new NoMealUserException(user.TelegramId, mealId);
        
        return userMealById;
    }
    
    private async Task FoodExistsGuard(long foodId)
    {
        var food = await _unitOfWork.FoodRepository.ExistAsync(foodId);
        if (food == false)
            throw new FoodNotFoundException(foodId);
    }
    
    private void FoodExistsInMealWithDayPartGuard(User user, long mealId, long foodId, long dayPartId)
    {
        if (user.Meals.Single(m => m.Id == mealId).Foods
            .Exists(mf => mf.MealId == mealId && mf.DayPartId == dayPartId))
            throw new FoodExistsInMealDayPartException(mealId, foodId, dayPartId);
    }

    private MealFood GetUserFoodByMeal(User user, long mealId, long foodId, long dayPartId)
    {
        var userMealById = GetMealById(user, mealId);
        var userMealFoods = userMealById.Foods.SingleOrDefault(mf => mf.FoodId == foodId && mf.DayPartId == dayPartId)
                            ?? throw new NoFoodUserException(user.TelegramId, mealId, foodId, dayPartId);

        return userMealFoods;
    }
}