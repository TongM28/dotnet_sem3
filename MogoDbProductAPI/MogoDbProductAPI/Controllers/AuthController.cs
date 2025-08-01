using Microsoft.AspNetCore.Mvc;
using MogoDbProductAPI.Domain.Model;
using MogoDbProductAPI.Models;
using MogoDbProductAPI.Service;
namespace MogoDbProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = _authService.Login(request);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid email or password.");
            }
        }
    }
}

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    //[HttpPost("register")]
    //public async Task<IActionResult> Register(UserDto dto)
    //{
    //    var user = new User { Username = dto.Username, Email = dto.Email };
    //    var created = await _userService.Register(user, dto.Password);
    //    return Ok(new { created.Id, created.Username });
    //}

    //[HttpPost("login")]
    //public async Task<IActionResult> Login(LoginDto dto)
    //{
    //    var token = await _userService.Login(dto.Email, dto.Password);
    //    if (token == null) return Unauthorized();
    //    return Ok(new { token });
    //}
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDto dto)
    {
        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            Role = string.IsNullOrEmpty(dto.Role) ? "User" : dto.Role // Nếu không truyền, gán User
        };
        var created = await _userService.Register(user, dto.Password);
        return Ok(new { created.Id, created.Username, created.Role });
    }

}

//public record UserDto(string Username, string Email, string Password);
public record UserDto(string Username, string Email, string Password, string Role); // Thêm Role

public record LoginDto(string Email, string Password);
