using ApplicationCore.Endpoints.Users;
using ApplicationCore.Entities;
using Ardalis.GuardClauses;
using CloudMarket.Helpers;
using CloudMarket.Interfaces;
using Refit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudMarket.Services
{
    public class ProfileService : BaseRefitService
    {
        private HttpClient httpClient;
        private string _username;
        private User _currentUser { get; set; }
        private IProfileService _profileApi;

        public ProfileService()
        {
            httpClient = new HttpClient(new AuthenticatedHttpClientHandler(GetToken));
            httpClient.BaseAddress = new Uri(ApiEndpointUrls.MagShopUrl);
            _profileApi = RestService.For<IProfileService>(httpClient);
        }
        public async Task<User> GetUserAsync(CancellationToken cancellationToken)
        {
            try
            {
                var response = await MakeRequest(ctx => _profileApi.GetUserAsync(ctx), cancellationToken);
                Guard.Against.Null(response, nameof(response));

                return response.User;
            }
            catch (Exception ex)
            {
                //Debugger.Break();
                return null;
            }
        }
    }
}
