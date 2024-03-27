using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestOne.Contexts;
using TestOne.Models;

namespace TestOne.Repositories
{
    public class NoteRepository : IDataRepository<Note>
    {
        private readonly NoteDbContext _context;
        public NoteRepository(NoteDbContext context)
        {
            _context = context;
        }
        public async Task<Note> AddAsync([FromBody]Note note)
        {
            if (_context.Notes == null) return null!;

            await _context.Notes.AddAsync(note);
            _context.SaveChanges();

            return note;
        }

        public async Task<int> DeleteAsync(int id)
        {
            if (_context.Notes == null) return 0;

            var data = await _context.Notes.FindAsync(id);
            if(data == null) return 0;

            _context.Notes.Remove(data);
            _context.SaveChanges();

            return id;
        }

        public async Task<Note?> GetAsync(int id)
        {
            if(_context.Notes == null) return null!;

            var data = await _context.Notes.FindAsync(id);
            return data;
        }

        public async Task<IEnumerable<Note>> GetListAsync()
        {
            if (_context.Notes == null) return null!;

            var data = await _context.Notes.ToListAsync();

            return data;
        }

        public async Task<Note?> UpdateAsync(Note note)
        {
            if (_context.Notes == null) return null!;

            var selectedNote = await _context.Notes.FindAsync(note.Id);
            if (selectedNote == null) return null!;

            selectedNote.Title = note.Title;
            selectedNote.Content = note.Content;
            selectedNote.AuthorId = note.AuthorId;

            _context.SaveChanges();

            return note;
        }
    }
}
