using Microsoft.AspNetCore.Mvc;
using NutriFitLogBackend.Domain.DTOs.Trainings;
using NutriFitLogBackend.Domain.Services.Trainings;

namespace NutriFitLogBackend.Controllers.Trainings;

[ApiController]
[Route("[controller]")]
public class TrainingController : ControllerBase
{
    private readonly ITrainingService _trainingService;

    public TrainingController(ITrainingService trainingService)
    {
        _trainingService = trainingService;
    }

    [HttpPost("Create")]
    public async Task<ActionResult<TrainingDto>> CreateTraining([FromBody] CreateTrainingDto dto)
    {
        var createdTraining = await _trainingService.CreateTraining(dto);
        return Ok(createdTraining);
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<TrainingDto>>> GetAllTrainings()
    {
        var trainings = await _trainingService.GetAllTrainings();
        return Ok(trainings);
    }

    [HttpPut("Update")]
    public async Task<ActionResult<TrainingDto>> UpdateTraining([FromBody] UpdateTrainingDto dto)
    {
        var updatedTraining = await _trainingService.UpdateTraining(dto);
        return Ok(updatedTraining);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteTraining(long id)
    {
        await _trainingService.DeleteTraining(id);
        return Ok();
    }
}