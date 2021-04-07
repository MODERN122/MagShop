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
        [Get("/api/users/{id}")]
        [Headers("Authorization: Bearer")]
        Task<GetUserResponse> GetUserByIdAsync(string id, CancellationToken ctx);
        [Get("/api/users")]
        [Headers("Authorization: Bearer")]
        Task<GetUserResponse> GetUserAsync(CancellationToken ctx);
    }
}
