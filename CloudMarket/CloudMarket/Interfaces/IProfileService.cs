using ApplicationCore.Endpoints.Users;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudMarket.Interfaces
{
    [Headers("Authorization: Bearer")]
    public interface IProfileService
    {
        [Get("/api/users/{id}")]
        Task<GetUserResponse> GetUserByIdAsync(string id, CancellationToken ctx);
        [Get("/api/users")]
        Task<GetUserResponse> GetUserAsync(CancellationToken ctx);
    }
}
