using NutriFitLogBackend.Domain.Repositories.Nutrition;
using NutriFitLogBackend.Domain.Repositories.Trainings;
using NutriFitLogBackend.Domain.Repositories.Users;

namespace NutriFitLogBackend.Domain;

public interface IUnitOfWork : IDisposable
{
    IUserRepository UserRepository { get; set; }
    ITrainingRepository TrainingRepository { get; set; }
    INutritionRepository NutritionRepository { get; set; }

    Task<int> SaveAsync();
}