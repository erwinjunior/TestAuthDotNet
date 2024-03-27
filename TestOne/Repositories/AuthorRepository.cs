using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestOne.Contexts;
using TestOne.Models;

namespace TestOne.Repositories
{
    public class AuthorRepository : IDataRepository<Author>
    {
        private readonly NoteDbContext _context;
        public AuthorRepository(NoteDbContext context)
        {
            _context = context;
        }
        public async Task<Author> AddAsync([FromBody] Author data)
        {
            if (_context.Authors == null) return null!;

            await _context.Authors.AddAsync(data);
            _context.SaveChanges();

            return data;
        }

        public async Task<int> DeleteAsync(int id)
        {
            if(_context.Authors == null) return 0;

            var data = await _context.Authors.FindAsync(id);
            if(data == null) return 0;

            _context.Authors.Remove(data);
            _context.SaveChanges();

            return id;
        }

        public async Task<Author?> GetAsync(int id)
        {
            if (_context.Authors == null) return null!;

            var data = await _context.Authors.FindAsync(id);
            if(data == null) return null!;

            return data;

        }

        public async Task<IEnumerable<Author>> GetListAsync()
        {
            if (_context.Authors == null) return null!;

            var data = await _context.Authors.ToListAsync();

            return data;
        }

        public async Task<Author?> UpdateAsync(Author data)
        {
            if (_context.Authors == null) return null!;

            var author = await _context.Authors.FindAsync(data.Id);
            if(author == null) return null!;

            author.AuthorName = data.AuthorName;
            _context.SaveChanges();

            return data;
        }
    }
}
