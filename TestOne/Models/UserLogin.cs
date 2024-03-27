using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TestOne.Models
{
    public class UserLogin
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
