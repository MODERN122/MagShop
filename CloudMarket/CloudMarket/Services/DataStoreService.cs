using ApplicationCore.Endpoints.Authentication;
using ApplicationCore.Endpoints.Baskets;
using ApplicationCore.Endpoints.Products;
using ApplicationCore.Endpoints.Users;
using ApplicationCore.Entities;
using Ardalis.GuardClauses;
using CloudMarket.Helpers;
using CloudMarket.Interfaces;
using CloudMarket.Models;
using Polly;
using Refit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CloudMarket.Services
{
    public class DataStoreService: BaseRefitService
    {
        private HttpClient httpClient;
        private IMagShopApi _magShopApi;

        public DataStoreService()
        {
            httpClient = new HttpClient(new AuthenticatedHttpClientHandler(GetToken));
            httpClient.BaseAddress = new Uri(ApiEndpointUrls.MagShopUrl);
            _magShopApi = RestService.For<IMagShopApi>(httpClient);
        }
        public async Task<bool> LoginUsernameAsync(string username, string password, CancellationToken cancellationToken)
        {
            var response = await MakeRequest(ctx => _magShopApi.AuthenticateAsync(new AuthenticationRequest() 
                { Username = username, Password = password }, ctx), cancellationToken);

            if (response!=null && response.Result)
            {
                _token = response.Token;
                await SecureStorage.SetAsync("token", response.Token);
                await SecureStorage.SetAsync("id", response.Username);
                _username = response.Username;
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<CreateUserResponse> RegisterUserAsync(CreateUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await MakeRequest(ctx => _magShopApi.CreateUserAsync(request, ctx), cancellationToken);
                Guard.Against.Null(response, nameof(response));
                _token = response.Token;

                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<ProductPreview>> GetListProductsAsync(GetProductsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await MakeRequest(ctx => _magShopApi.GetProductsAsync(request, ctx), cancellationToken);
                Guard.Against.Null(response, nameof(response));
                
                return response.Products;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<BasketItemResponse>> GetBasketItems(CancellationToken cancellationToken)
        {
            try
            {
                _username = await SecureStorage.GetAsync("id");
                var response = await MakeRequest(ctx => _magShopApi.GetUserBasketAsync(_username, ctx), cancellationToken);
                Guard.Against.Null(response, nameof(response));
                return response.BasketItems;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
