using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAuthRole.Constants;
using TestAuthRole.Models;
using TestAuthRole.Repositories;

namespace TestAuthRole.Controllers
{
    [Controller]
    [Route("v1/api/blog")]
    public class BlogController : Controller
    {
        private readonly IBlogRepository repo;
        public BlogController(IBlogRepository repo)
        {
            this.repo = repo;
        }

        [Authorize(Roles = $"{UserRoleInfo.ADMIN}")]
        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            if (repo == null) return NotFound();

            var data = await repo.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("({id})")]
        [Authorize(Roles = $"{UserRoleInfo.ADMIN}")]
        public async Task<IActionResult> Get(int id)
        {
            if (repo == null) return NotFound();
            var result = await repo.GetAsync(id);

            return result.Data != null ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Blog model)
        {
            if (repo == null) return NotFound();

            var result = await repo.AddAsync(model);

            return result.Data ? CreatedAtAction(nameof(Get), new {id = model.BlogId}, model) 
                : BadRequest(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (repo == null) return NotFound();

            var result = await repo.DeleteAsync(id);

            return result.Data ? Ok(result): BadRequest(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Blog model)
        {
            if (repo == null) return NotFound();

            var result = await repo.UpdateAsync(model);
            
            return result.Data ? Ok(result) : BadRequest(result);
        }
    }
}
