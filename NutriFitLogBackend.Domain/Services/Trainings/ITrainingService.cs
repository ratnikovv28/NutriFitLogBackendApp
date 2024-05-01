using NutriFitLogBackend.Domain.DTOs.Trainings;

namespace NutriFitLogBackend.Domain.Services.Trainings;

public interface ITrainingService
{
    Task<TrainingDto> CreateTraining(CreateTrainingDto createTrainingDto);
    Task<IEnumerable<TrainingDto>> GetAllTrainings();
    Task<TrainingDto> UpdateTraining(UpdateTrainingDto updateTrainingDto);
    Task DeleteTraining(long id);
}