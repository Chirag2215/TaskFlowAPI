using Ecommerce.Data;
using Ecommerce.DTOs.AuthDTO;
using Ecommerce.Model;
using Ecommerce.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System.Threading.Tasks;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TaskManageDataContext _dbcontext;
        private readonly JwtService _jwtService;
        private readonly ILogservice _logservice;
        public AuthController(TaskManageDataContext dbcontext,JwtService jwtService,ILogservice logservice)
        {
            _dbcontext = dbcontext;
            _jwtService = jwtService;
            _logservice = logservice;   
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("invalid data");

            }
            var user = await _dbcontext.Users
           .Include(u => u.UserRoles)
           .FirstOrDefaultAsync(u => u.UserEmail == dto.UserEmail);

            if (user == null) {
                return BadRequest("user not found");
                    }
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
            if (!isPasswordValid) {
                return BadRequest("password is incorrect");
            }

            var token = _jwtService.GenerateToken(user);
         
            await _logservice.AddLog("User Logged In",user.UserId);
            return Ok(new
            {
                token = token
            });
           
        }
        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegRequestDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data validation errot");
            }
            var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.UserEmail == dto.UserEmail);
            if (user != null)
            {
                return BadRequest("user is alredy exist ");
            }
            if (dto == null) {
                return BadRequest("User not null");
            }
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var validateRoleIds = await _dbcontext.Roles
                .Where(r => dto.RoleIds.Contains(r.RoleId)).Select(r=>r.RoleId).ToListAsync();

            if(validateRoleIds.Count != dto.RoleIds.Count)
            {
                return BadRequest("Invalid role id ");
            }
            User userToCreate = new User
            {
                UserEmail = dto.UserEmail,
                UserName = dto.UserName,
                Password = hashedPassword,
                UserRoles=dto.RoleIds.Select(RoleId => new UserRole { 
                RoleId = RoleId}).ToList(),
               

            };
               await _dbcontext.Users.AddAsync(userToCreate);
                await _dbcontext.SaveChangesAsync();

            //  var token = _jwtService.GenerateToken(dto);
            return Ok("reg success");
        }
    }
}
