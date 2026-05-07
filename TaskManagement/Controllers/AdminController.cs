using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        
        [Authorize]
        [HttpGet("UserData")]
        public IActionResult UserData()
        {
            return Ok("Login user access");
        }
        [Authorize(Roles = "5")]
        [HttpGet("AdminData")]
        public IActionResult AdminData()
        {
            return Ok("Only admin");
        }
    }
}
