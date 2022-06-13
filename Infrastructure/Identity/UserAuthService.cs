using ApplicationCore.GraphQLEndpoints;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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

        public async Task<Token> RegisterSellerAsync(UserAuthAccess userAccess)
        {
            return await AddUser(userAccess, Constants.ConstantsAPI.SELLERS);
        }

        public async Task<Token> RegisterUserAsync(UserAuthAccess userAccess)
        {
            return await AddUser(userAccess, Constants.ConstantsAPI.USERS);
        }
        public async Task<Token> RegisterAdministratorAsync(UserAuthAccess userAccess)
        {
            return await AddUser(userAccess, Constants.ConstantsAPI.ADMINISTRATORS);
        }

        private async Task<Token> AddUser(UserAuthAccess userAccess, string role)
        {
            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>()
                                       .CreateScope())
            {
                using UserManager<UserAuthAccess> userManager = serviceScope.ServiceProvider.GetService<UserManager<UserAuthAccess>>();

                var user1 = await userManager.FindByNameAsync(userAccess.UserName);
                if (user1 != null)
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

        public async Task<Token> GetTokenAsync(string userName)
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
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = GetRefreshToken(userName);
            var accessToken = tokenHandler.WriteToken(token);
            return new Token { AccessToken = accessToken, RefreshToken = refreshToken };

        }

        private string GetRefreshToken(string id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Constants.ConstantsAPI.JWT_SECRET_KEY);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("isRefresh", "true"),
                    new Claim("id", id),
                }),

                Expires = DateTime.UtcNow.AddDays(100),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = tokenHandler.WriteToken(securityToken);
            return refreshToken;
        }

        public async Task<Token> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters();
                SecurityToken validatedToken;
                //TODO maybe need handle if token is not valid
                var principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out validatedToken);

                var jwtToken = tokenHandler.ReadJwtToken(refreshToken);
                if (jwtToken.ValidTo < DateTime.UtcNow)
                {
                    throw new SecurityTokenExpiredException();
                }
                bool isRefresh = false;
                var isRefreshStr = jwtToken.Claims.FirstOrDefault(_ => _.Type == "isRefresh").Value;
                if (bool.TryParse(isRefreshStr, out isRefresh))
                {
                    if (isRefresh == false)
                    {
                        throw new Exception("This is not a refresh token!");
                    }

                    var id = jwtToken.Claims.FirstOrDefault(c => c.Type == "id").Value;

                    return await GetTokenAsync(id);
                }
                else
                {
                    throw new Exception("A bad token was sent!");
                }
            }
            catch (SecurityTokenExpiredException)
            {
                throw new Exception("A token was expired!");
            }
            catch (Exception)
            {
                throw new Exception("A bad token was sent!");
            }

        }

        private TokenValidationParameters GetValidationParameters()
        {
            var key = Encoding.ASCII.GetBytes(Constants.ConstantsAPI.JWT_SECRET_KEY);
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }
    }
}
