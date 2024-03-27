using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TestAuthRole.Models
{
    [PrimaryKey(nameof(Username), nameof(RoleId))]
    public class UserRole
    {
        public string Username { get; set; } = "";
        public int RoleId { get; set; }
    }
}
