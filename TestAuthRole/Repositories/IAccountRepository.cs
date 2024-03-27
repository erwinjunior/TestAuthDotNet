using Microsoft.AspNetCore.Identity;
using TestAuthRole.Models;

namespace TestAuthRole.Repositories
{
    public interface IAccountRepository
    {
        Task<ApiResponse<string>> LoginAsync(LoginModel model);
        Task<ApiResponse<bool>> RegisterAsync(RegisterModel model);
    }
}
