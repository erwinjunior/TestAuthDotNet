using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestOne.Contexts;
using TestOne.Models;
using TestOne.Repositories;

namespace TestOne.Controllers
{
    [ApiController]
    [Route("v1/api/Author")]
    public class AuthorController : ControllerBase
    {
        private readonly IDataRepository<Author> _repo;
        public AuthorController(IDataRepository<Author> repo)
        {
            _repo = repo;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            if (_repo == null) return NotFound();

            try
            {
                var data = await _repo.GetListAsync();
                return Ok(data);
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
                return data != null ? Ok(data) : BadRequest("Data is not valid");
            }
            catch (Exception ex)
            {
                return BadRequest($"Get detail failed, {ex.Message}");
            }
        }

        [HttpPost("post")]
        public async Task<IActionResult> Post([FromBody] Author author)
        {
            if (_repo == null) return NotFound();

            try
            {
                var data = await _repo.AddAsync(author);
                return CreatedAtAction(nameof(Get), new {id = author.Id}, author);
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
                int deleteAuthorId = await _repo.DeleteAsync(id);
                return deleteAuthorId != 0 ? Ok(deleteAuthorId) : BadRequest("Data is not valid");
            }
            catch (Exception ex)
            {
                return BadRequest($"Delete failed, {ex.Message}");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(Author author)
        {
            if(_repo == null) return NotFound();

            try
            {
                var data = await _repo.UpdateAsync(author);
                return data != null ? Ok(data) : BadRequest("Data is not valid");
            }
            catch(Exception ex)
            {
                return BadRequest($"Update failed, {ex.Message}");
            }
        }
    }
}
