using Microsoft.AspNetCore.Mvc;
using TestAuthRole.Models;

namespace TestAuthRole.Repositories
{
    public interface IBlogRepository
    {
        Task<ApiResponse<List<Blog>>> GetAllAsync();
        Task<ApiResponse<Blog>> GetAsync(int id);
        Task<ApiResponse<bool>> AddAsync([FromBody]Blog model);
        Task<ApiResponse<bool>> UpdateAsync([FromBody]Blog model);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}
