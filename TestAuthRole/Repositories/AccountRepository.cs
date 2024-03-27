using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestAuthRole.Constants;
using TestAuthRole.Contexts;
using TestAuthRole.Models;

namespace TestAuthRole.Repositories
{
    public class AccountRepository : IAccountRepository
    {

        private readonly IConfiguration configuration;
        private readonly AppDbContext context;
        public AccountRepository(IConfiguration configuration, AppDbContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResponse<string>> LoginAsync(LoginModel model)
        {
            // check not exists username
            var user = await context.Users.FindAsync(model.Username);
            if (user == null) return new ApiResponse<string> 
            {
                Message = "Username is not exists",
                Data = "",
            };

            // check wrong password
            if (model.Password != user.Password) return new ApiResponse<string>
            {
                Message = "Password wrong",
                Data = "",
            };

            var claims = new List<Claim>
            {
                new Claim(nameof(user.Username), user.Username),
                new Claim(nameof(user.Email), user.Email),
            };

            var userRoleIds = context.UserRoles.Where(item => item.Username == user.Username)
                .Select(item => item.RoleId).ToList();

            var roles = context.Roles.ToList();
            roles.ForEach(role =>
            {
                if (userRoleIds.Contains(role.RoleId))
                    claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
            });

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: configuration["Jwt:ValidIssuer"],
                audience: configuration["Jwt:ValidAudience"],
                expires: DateTime.Now.AddHours(2),
                claims: claims,
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new ApiResponse<string>
            {
                Message = "Login success",
                Data = accessToken
            };

        }
        /// <summary>
        /// Register
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<bool>> RegisterAsync(RegisterModel model)
        {
            // check exists username
            var existsUser = await context.Users.FindAsync(model.Username);
            if (existsUser != null) return new ApiResponse<bool>
            {
                Message = "Username is exists",
                Data = false
            };

            // add new user
            await context.Users.AddAsync(model);
            context.SaveChanges();

            // add role if not exists in db
            // default role {RoleId = 1, RoleName = user}
            var defaultRole = context.Roles.Where(r => r.RoleId == UserRoleInfo.DEFAULT_ROLE_ID)
                .FirstOrDefault();
            if(defaultRole == null)
            {
                await context.Roles.AddAsync(new Role
                {
                    RoleName = UserRoleInfo.USER
                });
                context.SaveChanges();
            }

            // add role for user
            // default new account's role is user
            await context.UserRoles.AddAsync(new UserRole
            {
                Username = model.Username,
                RoleId = UserRoleInfo.DEFAULT_ROLE_ID,
            });
            context.SaveChanges();

            return new ApiResponse<bool>
            {
                Message = "Register successfully",
                Data = true
            };
        }
    }
}
