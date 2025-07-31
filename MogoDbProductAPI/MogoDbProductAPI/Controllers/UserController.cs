using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MogoDbProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // Chỉ User mới truy cập được
        [HttpGet("profile")]
        [Authorize(Roles = "User")]
        public IActionResult GetProfile()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(new
            {
                message = "This is for users only",
                accessedBy = username
            });
        }

        // Cả Admin và User đều truy cập được
        [HttpGet("shared")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetSharedContent()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            return Ok(new
            {
                message = "This endpoint is accessible by Admin or User",
                yourRole = role
            });
        }
    }
}
