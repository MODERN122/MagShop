using ApplicationCore.GraphQLEndpoints;
using Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IUserAuthService
    {
        Task<Token> GetTokenAsync(string userName);
        Task<Token> RefreshTokenAsync(string refreshToken);
        Task<Token> RegisterUserAsync(UserAuthAccess userAccess);
        Task<Token> RegisterSellerAsync(UserAuthAccess userAccess);
    }
}
