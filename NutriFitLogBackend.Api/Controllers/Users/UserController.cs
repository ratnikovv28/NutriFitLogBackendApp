using Microsoft.AspNetCore.Mvc;
using NutriFitLogBackend.Domain.DTOs.Users;
using NutriFitLogBackend.Domain.Services;

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
}