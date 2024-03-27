using Microsoft.AspNetCore.Mvc;
using TestAuthRole.Models;
using TestAuthRole.Repositories;

namespace TestAuthRole.Controllers
{
    [Controller]
    [Route("v1/api/account")]
    public class AccountController : Controller
    {
        private readonly IAccountRepository repo;
        public AccountController(IAccountRepository repo)
        {
            this.repo = repo;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (repo == null) return NotFound();

            var result = await repo.RegisterAsync(model);

            return result.Data ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (repo == null) return NotFound();

            var result = await repo.LoginAsync(model);

            return result.Data != null ? Ok(new
            {
                message = result.Message,
                token = result.Data
            }) : BadRequest(result);
        }
    }
}
