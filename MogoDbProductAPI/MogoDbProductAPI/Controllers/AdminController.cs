using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MogoDbProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        // Chỉ admin mới truy cập được
        [HttpGet("secret")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetSecret()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(new
            {
                message = "This is a secret admin-only endpoint",
                accessedBy = username
            });
        }

        // Test lấy thông tin người dùng từ JWT
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                username,
                email,
                role
            });
        }
    }
}
