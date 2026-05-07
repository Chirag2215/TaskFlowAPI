using Ecommerce.DTOs.AuthDTO;
using Ecommerce.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Services

{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public String GenerateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim("UserId",user.UserId.ToString()),
               new Claim(ClaimTypes.Email, user.UserEmail),
               new Claim(ClaimTypes.Role, user.UserRoles.First().RoleId.ToString())

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                  issuer: _configuration["JWt:Issuer"],
                  audience: _configuration["JWT:Issuer"],
                  claims :claims,
                  expires:DateTime.Now.AddMinutes(60),
                  signingCredentials:creds

                );
              
           
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
