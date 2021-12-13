using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IServiceProvider _serviceProvider;

        public UserAuthService(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }

        public async Task<string> RegisterSellerAsync(UserAuthAccess userAccess)
        {
            return await AddUser(userAccess, Constants.ConstantsAPI.SELLERS);
        }

        public async Task<string> RegisterUserAsync(UserAuthAccess userAccess)
        {
            return await AddUser(userAccess, Constants.ConstantsAPI.USERS);
        }
        public async Task<string> RegisterAdministratorAsync(UserAuthAccess userAccess)
        {
            return await AddUser(userAccess, Constants.ConstantsAPI.ADMINISTRATORS);
        }

        private async Task<string> AddUser(UserAuthAccess userAccess, string role)
        {
            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>()
                                       .CreateScope())
            {
                using UserManager<UserAuthAccess> userManager = serviceScope.ServiceProvider.GetService<UserManager<UserAuthAccess>>();

                var user1 = await userManager.FindByNameAsync(userAccess.UserName);
                if(user1!=null)
                {
                    throw new InvalidOperationException("Пользователь уже существует");
                }
                await userManager.CreateAsync(userAccess);
                var user = await userManager.FindByNameAsync(userAccess.UserName);
                await userManager.AddToRoleAsync(user, role);
            }
            var token = await GetTokenAsync(userAccess.UserName);
            return token;
        }
        public async Task<string> GetTokenAsync(string userName)
        {
            using var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>()
                           .CreateScope();
            using UserManager<UserAuthAccess> userManager = serviceScope.ServiceProvider.GetService<UserManager<UserAuthAccess>>();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Constants.ConstantsAPI.JWT_SECRET_KEY);
            var user = await userManager.FindByNameAsync(userName);
            var roles = await userManager.GetRolesAsync(user);
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}
