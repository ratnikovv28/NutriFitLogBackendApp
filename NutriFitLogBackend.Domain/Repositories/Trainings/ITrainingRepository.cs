using NutriFitLogBackend.Domain.Entities.Trainings;

namespace NutriFitLogBackend.Domain.Repositories.Trainings;

public interface ITrainingRepository
{
    Task<Training> GetByIdAsync(long id);
    Task<IReadOnlyCollection<Training>> GetAllByTelegramIdAsync(long telegramId);
    Task<IReadOnlyCollection<Training>> GetAllAsync();
    Task<Training> AddAsync(Training training);
    void UpdateAsync(Training training);
    void DeleteAsync(Training training);
}