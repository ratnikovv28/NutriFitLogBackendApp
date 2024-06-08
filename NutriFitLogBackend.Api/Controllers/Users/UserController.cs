using Microsoft.AspNetCore.Mvc;
using NutriFitLogBackend.Domain.DTOs.Users;
using NutriFitLogBackend.Domain.DTOs.Users.RequestDTOs;
using NutriFitLogBackend.Domain.Services.Users;

namespace NutriFitLogBackend.Controllers.Users;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }
    
    [HttpPost("Get")]
    public async Task<ActionResult<UserDto>> Get([FromBody]long telegramId)
    {
        var user = await _userService.GetUserByTelegramId(telegramId);
        return Ok(user);
    }
    
    [HttpPost("GetTrainers")]
    public async Task<ActionResult<IReadOnlyCollection<UserDto>>> GetTrainers([FromBody] long telegramId)
    {
        var user = await _userService.GetTrainers(telegramId);
        return Ok(user);
    }
    
    [HttpPost("Create")]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto dto)
    {
        var createdUser = await _userService.CreateUser(dto);
        return Ok(createdUser);
    }
    
    [HttpPost("GetStudents")]
    public async Task<ActionResult<IReadOnlyCollection<UserDto>>> GetStudents(TrainerStudentsDto dto)
    {
        var user = await _userService.GetStudents(dto.TelegramId, dto.AreActive);
        return Ok(user);
    }
    
    [HttpPost("AddStudentTrainer")]
    public async Task<ActionResult> AddStudentTrainer([FromBody] StudentTrainerDto dto)
    {
        await _userService.AddStudentToTrainer(dto.StudentTelegramId, dto.TrainerTelegramId);
        return Ok();
    }
    
    [HttpPost("CreateStudentTrainer")]
    public async Task<ActionResult> CreateStudentTrainer([FromBody] StudentTrainerDto dto)
    {
        await _userService.CreateStudentTrainer(dto.StudentTelegramId, dto.TrainerTelegramId);
        return Ok();
    }
    
    [HttpPut("UpdateTrainerStatus")]
    public async Task<ActionResult<UserDto>> UpdateUserTrainerStatus([FromBody] CreateUserDto dto)
    {
        var updatedUser = await _userService.UpdateTrainerStatus(dto.TelegramId);
        return Ok(updatedUser);
    }
    
    [HttpDelete("DeleteStudentTrainer")]
    public async Task<IActionResult> Delete([FromBody] StudentTrainerDto dto)
    {
        await _userService.DeleteStudentTrainerRelationShip(dto.StudentTelegramId, dto.TrainerTelegramId);
        return Ok();
    }
}