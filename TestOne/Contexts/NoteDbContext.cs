using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestOne.Models;

namespace TestOne.Contexts
{
    public class NoteDbContext: IdentityDbContext<AppUser>
    {
        public NoteDbContext(DbContextOptions<NoteDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Note> Notes { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}
