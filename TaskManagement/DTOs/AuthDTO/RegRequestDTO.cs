using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTOs.AuthDTO
{
    public class RegRequestDTO
    {
       [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string UserEmail { get; set; } = string.Empty;
        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }= string.Empty;
        [Required]
        public List<int>? RoleIds { get; set; }
    }
}
