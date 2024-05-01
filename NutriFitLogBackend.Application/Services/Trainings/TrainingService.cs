using AutoMapper;
using NutriFitLogBackend.Domain;
using NutriFitLogBackend.Domain.DTOs.Trainings;
using NutriFitLogBackend.Domain.Entities.Trainings;
using NutriFitLogBackend.Domain.Exceptions;
using NutriFitLogBackend.Domain.Services.Trainings;

namespace NutriFitLogBackend.Application.Services.Trainings;

public class TrainingService : ITrainingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TrainingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TrainingDto> CreateTraining(CreateTrainingDto createTrainingDto)
    {
        var training = _mapper.Map<Training>(createTrainingDto);
        await _unitOfWork.TrainingRepository.AddAsync(training);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<TrainingDto>(training);
    }

    public async Task<IEnumerable<TrainingDto>> GetAllTrainings()
    {
        var trainings = await _unitOfWork.TrainingRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<TrainingDto>>(trainings);
    }

    public async Task<TrainingDto> UpdateTraining(UpdateTrainingDto updateTrainingDto)
    {
        var training = await _unitOfWork.TrainingRepository.GetByIdAsync(updateTrainingDto.Id);
        if (training == null)
            throw new TrainingNotFoundException();

        _mapper.Map(updateTrainingDto, training);
        await _unitOfWork.TrainingRepository.UpdateAsync(training);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<TrainingDto>(training);
    }

    public async Task DeleteTraining(long id)
    {
        var training = await _unitOfWork.TrainingRepository.GetByIdAsync(id);
        if (training == null)
            throw new TrainingNotFoundException();

        await _unitOfWork.TrainingRepository.DeleteAsync(training);
        await _unitOfWork.SaveAsync();
    }
}