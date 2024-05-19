using NutriFitLogBackend.Domain.Repositories.Nutrition;
using NutriFitLogBackend.Domain.Repositories.Trainings;
using NutriFitLogBackend.Domain.Repositories.Users;

namespace NutriFitLogBackend.Domain;

public interface IUnitOfWork : IDisposable
{
    IUserRepository UserRepository { get; set; }
    IStudentTrainerRepository StudentTrainerRepository { get; set; }
    
    ITrainingRepository TrainingRepository { get; set; }
    ITrainingExerciseRepository TrainingExerciseRepository { get; set; }
    IExercisesRepository ExercisesRepository { get; set; }
    ISetRepository SetRepository { get; set; }
    
    INutritionRepository NutritionRepository { get; set; }
    IFoodRepository FoodRepository { get; set; }
    IDayPartRepository DayPartRepository { get; set; }
    IMealFoodRepository MealFoodRepository { get; set; }

    Task SaveAsync();
}