using Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IUserAuthService
    {
        Task<string> GetTokenAsync(string userName);
        Task<string> RegisterUserAsync(UserAuthAccess userAccess);
        Task<string> RegisterSellerAsync(UserAuthAccess userAccess);
    }
}
