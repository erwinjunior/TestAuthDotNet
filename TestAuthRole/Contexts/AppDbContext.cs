using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestAuthRole.Models;

namespace TestAuthRole.Contexts
{
    public class AppDbContext: IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Role> Roles {  get; set; }
        public DbSet<UserRole> UserRoles {  get; set; }
        public DbSet<Blog> Blogs { get; set; }
    }
}
