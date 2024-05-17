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
        
    [HttpPost("Create")]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto dto)
    {
        var createdUser = await _userService.CreateUser(dto);
        return Ok(createdUser);
    }
    
    [HttpGet("Get/{telegramId}")]
    public async Task<ActionResult<UserDto>> Get(long telegramId)
    {
        var user = await _userService.GetUserByTelegramId(telegramId);
        return Ok(user);
    }
    
    [HttpGet("GetTrainers/{telegramId}")]
    public async Task<ActionResult<IReadOnlyCollection<UserDto>>> GetTrainers(long telegramId)
    {
        var user = await _userService.GetTrainers(telegramId);
        return Ok(user);
    }
    
    [HttpPost("GetStudents")]
    public async Task<ActionResult<IReadOnlyCollection<UserDto>>> GetStudents(TrainerStudentsDto dto)
    {
        var user = await _userService.GetStudents(dto.TelegramId, dto.AreActive);
        return Ok(user);
    }
    
    [HttpPut("UpdateTrainerStatus")]
    public async Task<ActionResult<UserDto>> UpdateUserTrainerStatus([FromBody] CreateUserDto dto)
    {
        var updatedUser = await _userService.UpdateTrainerStatus(dto.TelegramId);
        return Ok(updatedUser);
    }
    
    [HttpGet("GetAll")]
    public async Task<ActionResult<UserDto>> GetAllUsers()
    {
        var users = await _userService.GetUsers();
        return Ok(users);
    }

    [HttpPut("Update")]
    public async Task<ActionResult<UserDto>> UpdateUser([FromBody] UpdateUserDto dto)
    {
        var updatedUser = await _userService.UpdateUser(dto);
        return Ok(updatedUser);
    }

    [HttpDelete("Delete/{telegramId}")]
    public async Task<IActionResult> Delete(long telegramId)
    {
        await _userService.DeleteUserByTelegramId(telegramId);
        return Ok();
    }
    
    [HttpDelete("DeleteStudentTrainer")]
    public async Task<IActionResult> Delete([FromBody] StudentTrainerDto dto)
    {
        await _userService.DeleteStudentTrainerRelationShip(dto.StudentTelegramId, dto.TrainerTelegramId);
        return Ok();
    }
    
    [HttpPost("AddStudentTrainer")]
    public async Task<ActionResult<UserDto>> AddStudentTrainer([FromBody] StudentTrainerDto dto)
    {
        await _userService.AddStudentToTrainer(dto.StudentTelegramId, dto.TrainerTelegramId);
        return Ok();
    }
    
    [HttpPost("CreateStudentTrainer")]
    public async Task<ActionResult<UserDto>> CreateStudentTrainer([FromBody] StudentTrainerDto dto)
    {
        await _userService.CreateStudentTrainer(dto.StudentTelegramId, dto.TrainerTelegramId);
        return Ok();
    }
}