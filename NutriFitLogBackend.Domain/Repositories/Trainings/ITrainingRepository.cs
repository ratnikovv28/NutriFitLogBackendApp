using NutriFitLogBackend.Domain.Entities.Trainings;

namespace NutriFitLogBackend.Domain.Repositories.Trainings;

public interface ITrainingRepository
{
    Task<Training> GetTrainingByIdAsync(long id);
    Task<IEnumerable<Training>> GetAllTrainingsAsync();
    Task<Training> AddTrainingAsync(Training training);
    Task UpdateTrainingAsync(Training training);
    Task DeleteTrainingAsync(Training training);
}