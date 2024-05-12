using NutriFitLogBackend.Domain;
using NutriFitLogBackend.Domain.Repositories.Nutrition;
using NutriFitLogBackend.Domain.Repositories.Trainings;
using NutriFitLogBackend.Domain.Repositories.Users;
using NutriFitLogBackend.Infrastructure.Database;

namespace NutriFitLogBackend.Infrastructure;

public class UnitOfWork : IUnitOfWork
{   
    private readonly NutriFitLogContext _nutriFitLogContext;

    public IUserRepository UserRepository { get; set; }
    public IActionRepository ActionRepository { get; set; }
    
    public ITrainingRepository TrainingRepository { get; set; }
    public ITrainingExerciseRepository TrainingExerciseRepository { get; set; }
    public IExercisesRepository ExercisesRepository { get; set; }
    public ISetRepository SetRepository { get; set; }
    
    public INutritionRepository NutritionRepository { get; set; }
    public IFoodRepository FoodRepository { get; set; }
    public IDayPartRepository DayPartRepository { get; set; }
    public IMealFoodRepository MealFoodRepository { get; set; }
    
    public UnitOfWork(
        NutriFitLogContext nutriFitLogContext,
        IUserRepository userRepository,
        ITrainingRepository trainingRepository,
        INutritionRepository nutritionRepository, 
        IActionRepository actionRepository,
        IExercisesRepository exercisesRepository,
        ISetRepository setRepository, 
        IFoodRepository foodRepository, 
        IDayPartRepository dayPartRepository, 
        ITrainingExerciseRepository trainingExerciseRepository, 
        IMealFoodRepository mealFoodRepository)
    {
        _nutriFitLogContext = nutriFitLogContext;
        UserRepository = userRepository;
        TrainingRepository = trainingRepository;
        NutritionRepository = nutritionRepository;
        ActionRepository = actionRepository;
        ExercisesRepository = exercisesRepository;
        SetRepository = setRepository;
        FoodRepository = foodRepository;
        DayPartRepository = dayPartRepository;
        TrainingExerciseRepository = trainingExerciseRepository;
        MealFoodRepository = mealFoodRepository;
    }
    
    public async Task<int> SaveAsync() => await _nutriFitLogContext.SaveChangesAsync();

    public void Dispose() => _nutriFitLogContext.Dispose();
}