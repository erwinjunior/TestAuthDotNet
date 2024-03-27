using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestOne.Helpers;
using TestOne.Models;
using TestOne.Repositories;

namespace TestOne.Controllers
{
    [ApiController]
    [Route("v1/api/Note")]
    public class NoteController : Controller
    {
        public IDataRepository<Note> _repo;
        public NoteController(IDataRepository<Note> repo)
        {
            _repo = repo;
        }

        [HttpGet("list")]
        [Authorize]

        public async Task<IActionResult> List()
        {
            if (_repo == null) return NotFound(); // status code = 404

            try
            {
                var data = await _repo.GetListAsync();
                return Ok(data); // status code = 200
            }
            catch (Exception ex)
            {
                return BadRequest($"Get list failed, {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (_repo == null) return NotFound();

            try
            {
                var data = await _repo.GetAsync(id);
                return data != null ? Ok(data) : BadRequest("Data is invalid");
            }
            catch (Exception ex)
            {
                return BadRequest($"Get detail failed ,{ex.Message}");
            }

        }

        [HttpPost("post")]
        public async Task<IActionResult> Post([FromBody] Note note)
        {
            if (_repo == null) return NotFound();

            try
            {
                var data = await _repo.AddAsync(note);
                return CreatedAtAction(nameof(Get), new { id = note.Id }, note);
            }
            catch (Exception ex)
            {
                return BadRequest($"Add failed, {ex.Message}");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_repo == null) return NotFound();

            try
            {
                var deleteNoteId = await _repo.DeleteAsync(id);
                return deleteNoteId != 0 ? Ok(deleteNoteId) : BadRequest("Note Id is invalid");
            }
            catch (Exception ex)
            {
                return BadRequest($"Delete failed, {ex.Message}");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(Note note)
        {
            if (_repo == null) return NotFound();

            try
            {
                var data = await _repo.UpdateAsync(note);
                return data != null ? Ok(data) : BadRequest("Data is invalid");
            }
            catch (Exception ex)
            {
                return BadRequest($"Update failed, {ex.Message}");
            }
        }
    }
}
