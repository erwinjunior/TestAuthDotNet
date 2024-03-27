using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAuthRole.Contexts;
using TestAuthRole.Models;

namespace TestAuthRole.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext context;
        public BlogRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<ApiResponse<bool>> AddAsync([FromBody] Blog model)
        {
            // check exists blog
            var existsBlog = await context.Blogs.FindAsync(model.BlogId);
            if (existsBlog != null) return new ApiResponse<bool>
            {
                Message = "Blog id is exists",
                Data = false
            };

            await context.Blogs.AddAsync(model);
            context.SaveChanges();

            return new ApiResponse<bool>
            {
                Message = "Add new blog successfully",
                Data = true
            };
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var deleteBlog = await context.Blogs.FindAsync(id);
            if (deleteBlog == null) return new ApiResponse<bool>
            {
                Message = "Blog is not exists",
                Data = false
            };

            context.Blogs.Remove(deleteBlog);
            context.SaveChanges();

            return new ApiResponse<bool>
            {
                Message = "Delete blog successfully",
                Data = true
            };
        }

        public async Task<ApiResponse<List<Blog>>> GetAllAsync()
        {
            var list = await context.Blogs.ToListAsync();

            return new ApiResponse<List<Blog>>
            {
                Message = "Get blogs successfully",
                Data = list
            };
        }

        public async Task<ApiResponse<Blog>> GetAsync(int id)
        {
            var blog = await context.Blogs.FindAsync(id);
            if (blog == null) return new ApiResponse<Blog>
            {
                Message = "Blog is not exists",
            };

            return new ApiResponse<Blog>
            {
                Message = "Get blog successfully",
                Data = blog
            };
        }

        public async Task<ApiResponse<bool>> UpdateAsync([FromBody] Blog model)
        {
            var blog = await context.Blogs.FindAsync(model.BlogId);
            if (blog == null) return new ApiResponse<bool>
            {
                Message = "Blog is not exists"
            };

            blog.Title = model.Title;
            blog.Content = model.Content;
            blog.AuthorName = model.AuthorName;

            context.SaveChanges();

            return new ApiResponse<bool>
            {
                Message = "Update successfully",
                Data = true
            };
        }
    }
}
