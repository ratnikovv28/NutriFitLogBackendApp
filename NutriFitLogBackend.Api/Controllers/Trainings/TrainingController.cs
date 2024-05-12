using Microsoft.AspNetCore.Mvc;
using NutriFitLogBackend.Domain.DTOs.Trainings;
using NutriFitLogBackend.Domain.DTOs.Trainings.RequestDTOs;
using NutriFitLogBackend.Domain.Services.Trainings;

namespace NutriFitLogBackend.Controllers.Trainings;

[ApiController]
[Route("[controller]")]
public class TrainingController : ControllerBase
{
    private readonly ITrainingService _trainingService;

    public TrainingController(ITrainingService trainingService)
    {
        _trainingService = trainingService ?? throw new ArgumentNullException(nameof(trainingService));
    }

    [HttpGet("GetAllExercises")]
    public async Task<ActionResult<IReadOnlyCollection<ExerciseDto>>> GetAllExercises()
    {
        return Ok(await _trainingService.GetAllExercisesAsync());
    }
    
    [HttpPost("GetUserTrainingByDate")]
    public async Task<ActionResult<TrainingDto>> GetUserTrainingByDate([FromBody] UserExercisesByDateDto dto)
    {
        return Ok(await _trainingService.GetUserExercisesByDateAsync(dto.TelegramId, dto.Date, dto.TrainerId));
    }
    
    [HttpPost("AddUserExercise")]
    public async Task<ActionResult> AddUserExercise([FromBody] CreateExerciseDto dto)
    {
        await _trainingService.AddExerciseAsync(dto.TelegramId, dto.TrainingId, dto.ExerciseId, dto.TrainerId);
        return Ok();
    }

    [HttpPut("UpdateSetsExercise")]
    public async Task<ActionResult> UpdateSetsExercise([FromBody] UpdateExerciseDto dto)
    {
        await _trainingService.UpdateSetsExerciseAsync(dto.TelegramId, dto.TrainingId, dto.ExerciseId, dto.Sets,
            dto.TrainerId);
        return Ok();
    }
    
    [HttpDelete("DeleteUserExercise")]
    public async Task<ActionResult> DeleteUserExercise([FromBody] DeleteUserExerciseDto dto)
    {
        await _trainingService.DeleteExerciseAsync(dto.TelegramId, dto.TrainingId, dto.ExerciseId, dto.TrainerId);
        return Ok();
    }

    [HttpDelete("DeleteSetsExercise")]
    public async Task<ActionResult> DeleteSetsExercise([FromBody] DeleteSetsExerciseDto dto)
    {
        await _trainingService.DeleteSetsExerciseAsync(dto.TelegramId, dto.TrainingId, dto.ExerciseId, dto.SetId,
            dto.TrainerId);
        return Ok();
    }
}