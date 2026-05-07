using Ecommerce.Data;
using Ecommerce.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly TaskManageDataContext _context;
        public RoleController(TaskManageDataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        
            {
           var roles = _context.Roles.ToList();
            return Ok(roles);
            }
        [HttpPost]
        public ActionResult Create([FromBody] Role role)
        {
            if(role== null)
            {
                return BadRequest();
            }
            Role roleToUpdate = new Role { RoleName=role.RoleName};
            _context.Roles.Add(roleToUpdate);
            _context.SaveChanges();
            return Ok("Yes Updated");
        }
    }
}
    
