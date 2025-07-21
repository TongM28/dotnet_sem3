using Microsoft.AspNetCore.Mvc;
using MogoDbProductAPI.Domain.Model;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDto dto)
    {
        var user = new User { Username = dto.Username, Email = dto.Email };
        var created = await _userService.Register(user, dto.Password);
        return Ok(new { created.Id, created.Username });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _userService.Login(dto.Email, dto.Password);
        if (token == null) return Unauthorized();
        return Ok(new { token });
    }
}

public record UserDto(string Username, string Email, string Password);
public record LoginDto(string Email, string Password);
