using Microsoft.AspNetCore.Mvc;
using TestOne.Models;

namespace TestOne.Repositories
{
    public interface IDataRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetListAsync();
        Task<T?> GetAsync(int id);
        Task<T> AddAsync([FromBody]T data);
        Task<int> DeleteAsync(int id);
        Task<T?> UpdateAsync(T data);

    }
}
