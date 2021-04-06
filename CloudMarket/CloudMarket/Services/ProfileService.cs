using ApplicationCore.Endpoints.Users;
using CloudMarket.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudMarket.Services
{
    public class ProfileService : IProfileService
    {
        public Task<GetUserResponse> GetUserAsync(string id, CancellationToken ctx)
        {
            throw new NotImplementedException();
        }
    }
}
