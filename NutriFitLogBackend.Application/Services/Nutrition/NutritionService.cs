using AutoMapper;
using NutriFitLogBackend.Domain;
using NutriFitLogBackend.Domain.DTOs.Nutrition;
using NutriFitLogBackend.Domain.Entities.Nutrition;
using NutriFitLogBackend.Domain.Exceptions;
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

    public async Task<MealDto> CreateMeal(CreateMealDto createMealDto)
    {
        var meal = _mapper.Map<Meal>(createMealDto);
        await _unitOfWork.NutritionRepository.AddMealAsync(meal);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<MealDto>(meal);
    }

    public async Task<IEnumerable<MealDto>> GetAllMeals()
    {
        var meals = await _unitOfWork.NutritionRepository.GetAllMealsAsync();
        return _mapper.Map<IEnumerable<MealDto>>(meals);
    }

    public async Task<MealDto> UpdateMeal(UpdateMealDto updateMealDto)
    {
        var meal = await _unitOfWork.NutritionRepository.GetMealByIdAsync(updateMealDto.Id);
        if (meal == null)
            throw new NutritionNotFoundException();

        _mapper.Map(updateMealDto, meal);
        await _unitOfWork.NutritionRepository.UpdateMealAsync(meal);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<MealDto>(meal);
    }

    public async Task DeleteMeal(long id)
    {
        var meal = await _unitOfWork.NutritionRepository.GetMealByIdAsync(id);
        if (meal == null)
            throw new NutritionNotFoundException();

        await _unitOfWork.NutritionRepository.DeleteMealAsync(meal);
        await _unitOfWork.SaveAsync();
    }
}