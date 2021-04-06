using ApplicationCore.Endpoints.Authentication;
using ApplicationCore.Endpoints.Baskets;
using ApplicationCore.Endpoints.Products;
using ApplicationCore.Endpoints.Users;
using ApplicationCore.Entities;
using Ardalis.GuardClauses;
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

namespace CloudMarket.Services
{
    public class DataStoreService
    {
        private string _token;
        private HttpClient httpClient;
        private string _username;
        private User _currentUser { get; set; }
        private IMagShopApi _magShopApi;

        public DataStoreService()
        {
            httpClient = new HttpClient(new AuthenticatedHttpClientHandler(GetToken));
            httpClient.BaseAddress = new Uri("https://0a96655a6eb0.ngrok.io");
            _magShopApi = RestService.For<IMagShopApi>(httpClient);
        }
        public async Task<bool> LoginUsernameAsync(string username, string password, CancellationToken cancellationToken)
        {
            var response = await MakeRequest(ctx => _magShopApi.AuthenticateAsync(new AuthenticationRequest() 
                { Username = username, Password = password }, ctx), cancellationToken);

            if (response!=null && response.Result)
            {
                _token = response.Token;
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
                var response = await MakeRequest(ctx => _magShopApi.GetUserBasketAsync(_username, ctx), cancellationToken);
                Guard.Against.Null(response, nameof(response));
                return response.BasketItems;
            }
            catch (Exception)
            {
                return null;
            }
        }


        class AuthenticatedHttpClientHandler : HttpClientHandler
        {
            private readonly Func<Task<string>> getToken;
            public AuthenticatedHttpClientHandler(Func<Task<string>> getToken)
            {
                if (getToken == null) throw new ArgumentNullException(nameof(getToken));
                this.getToken = getToken;
            }
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                // See if the request has an authorize header
                var auth = request.Headers.Authorization;
                if (auth != null)
                {
                    var token = await getToken().ConfigureAwait(false);
                    request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, token);
                }
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
        }
        private async Task<string> GetToken()
        {
            // The AcquireTokenAsync call will prompt with a UI if necessary
            // Or otherwise silently use a refresh token to return
            // a valid access token   
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluQG1pY3Jvc" +
                "29mdC5jb20iLCJyb2xlIjoiQWRtaW5pc3RyYXRvcnMiLCJuYmYiOjE2MTA0OTExNjMsImV4cCI6M" +
                "TYxMTA5NTk2MywiaWF0IjoxNjEwNDkxMTYzfQ.s87nOGyIr3DcBmHX--mJ_tWTTpWnQUr7hOdkFEOx3SY";
            //var token = await context.AcquireTokenAsync("http://my.service.uri/app", "clientId", new Uri("callback://complete"));
            return _token;
        }
        protected async Task<T> MakeRequest<T>(Func<CancellationToken, Task<T>> loadingFunction, CancellationToken cancellationToken)
        {
            Exception exception = null;
            var result = default(T);

            try
            {
                result = await Policy.Handle<WebException>().Or<HttpRequestException>()
                    .WaitAndRetryAsync(3, i => TimeSpan.FromMilliseconds(300), (ex, span) => exception = ex)
                    .ExecuteAsync(loadingFunction, cancellationToken);
            }
            catch (Exception e)
            {
                // Сюда приходят ошибки вроде отсутствия интернет-соединения или неправильной работы DNS
                exception = e;
            }
            //TODO: Обработать исключения или передать их дальше            
            return result;
        }
    }
}
