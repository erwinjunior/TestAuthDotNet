using Microsoft.AspNetCore.Mvc;
using TestOne.Models;
using TestOne.Repositories;

namespace TestOne.Controllers
{
    [Controller]
    [Route("v1/api/Account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepo;
        public AccountController(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(UserRegister model)
        {
            var result = await _accountRepo.RegisterAsync(model);

            return result.Succeeded ? Ok(model) : Unauthorized(result.Errors);
        }

        [Route("login")]
        [HttpGet]
        public async Task<IActionResult> Login(UserLogin model)
        {
            var result = await _accountRepo.LoginAsync(model);
            try
            {

                return !string.IsNullOrEmpty(result) ? Ok(result) : Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }
    }
}
