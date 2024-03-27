using System.ComponentModel.DataAnnotations;

namespace TestAuthRole.Models
{
    public class ApplicationUser
    {
        [Key]
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
