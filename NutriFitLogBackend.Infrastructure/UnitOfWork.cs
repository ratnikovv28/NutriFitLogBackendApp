using NutriFitLogBackend.Domain;
using NutriFitLogBackend.Domain.Repositories;
using NutriFitLogBackend.Domain.Repositories.Nutrition;
using NutriFitLogBackend.Domain.Repositories.Trainings;
using NutriFitLogBackend.Domain.Repositories.Users;
using NutriFitLogBackend.Infrastructure.Database;

namespace NutriFitLogBackend.Infrastructure;

public class UnitOfWork : IUnitOfWork
{   
    private readonly NutriFitLogContext _nutriFitLogContext;

    public IUserRepository UserRepository { get; set; }
    public ITrainingRepository TrainingRepository { get; set; }
    public INutritionRepository NutritionRepository { get; set; }
    
    public UnitOfWork(
        NutriFitLogContext nutriFitLogContext,
        IUserRepository userRepository,
        ITrainingRepository trainingRepository,
        INutritionRepository nutritionRepository)
    {
        _nutriFitLogContext = nutriFitLogContext;
        UserRepository = userRepository;
        TrainingRepository = trainingRepository;
        NutritionRepository = nutritionRepository;
    }
    
    public async Task<int> SaveAsync() => await _nutriFitLogContext.SaveChangesAsync();

    public void Dispose() => _nutriFitLogContext.Dispose();
}