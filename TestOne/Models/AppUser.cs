using Microsoft.AspNetCore.Identity;

namespace TestOne.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
