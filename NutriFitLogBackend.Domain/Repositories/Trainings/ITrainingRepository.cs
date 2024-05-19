using NutriFitLogBackend.Domain.Entities.Trainings;

namespace NutriFitLogBackend.Domain.Repositories.Trainings;

public interface ITrainingRepository
{
    Task<IReadOnlyCollection<Training>> GetAllByTelegramIdAsync(long telegramId);
    Task<Training> AddAsync(Training training);
}