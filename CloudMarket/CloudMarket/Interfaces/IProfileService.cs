using ApplicationCore.Endpoints.Users;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudMarket.Interfaces
{
    public interface IProfileService
    {
        [Get("/api/user/{id}")]
        [Headers("Authorization: Bearer")]
        Task<GetUserResponse> GetUserAsync(string id, CancellationToken ctx);
    }
}
