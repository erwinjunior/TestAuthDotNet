using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestOne.Helpers;
using TestOne.Models;

namespace TestOne.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountRepository(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }
        public async Task<string> LoginAsync(UserLogin model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
                return string.Empty;

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if(!passwordValid) return string.Empty;

            var result = await _signInManager.PasswordSignInAsync(model.UserName!, model.Password, false, false);
            
            if(!result.Succeeded)
            {
                return string.Empty;
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.UserName!),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, 
                    Guid.NewGuid().ToString()),
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var authenKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JWT:Secrect"]));

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                // token expiration time
                expires: DateTime.Now.AddHours(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256Signature)
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToken;
        }

        public async Task<IdentityResult> RegisterAsync(UserRegister model)
        {
            var user = new AppUser
            {
                UserName = model.UserName,
                FullName = model.FullName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
            {
                if(!await _roleManager.RoleExistsAsync(AppRole.USER))
                {
                    await _roleManager.CreateAsync(new IdentityRole(AppRole.USER));
                }

                await _userManager.AddToRoleAsync(user, AppRole.USER);
            }

            return result;
        }
    }
}
