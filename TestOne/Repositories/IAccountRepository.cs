using Microsoft.AspNetCore.Identity;
using TestOne.Models;

namespace TestOne.Repositories
{
    public interface IAccountRepository
    {
        Task<IdentityResult> RegisterAsync(UserRegister model);
        Task<string> LoginAsync(UserLogin model);
    }
}
