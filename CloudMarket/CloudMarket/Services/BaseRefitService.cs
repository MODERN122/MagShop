using ApplicationCore.Entities;
using Polly;
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
    public class BaseRefitService
    {
        protected string _username;
        protected User _currentUser { get; set; }
        protected string _token;

        protected class AuthenticatedHttpClientHandler : HttpClientHandler
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
        protected async Task<string> GetToken()
        {
            // The AcquireTokenAsync call will prompt with a UI if necessary
            // Or otherwise silently use a refresh token to return
            // a valid access token   
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluQG1pY3Jvc" +
                "29mdC5jb20iLCJyb2xlIjoiQWRtaW5pc3RyYXRvcnMiLCJuYmYiOjE2MTA0OTExNjMsImV4cCI6M" +
                "TYxMTA5NTk2MywiaWF0IjoxNjEwNDkxMTYzfQ.s87nOGyIr3DcBmHX--mJ_tWTTpWnQUr7hOdkFEOx3SY";

            token = await SecureStorage.GetAsync("token");
            //var token = await context.AcquireTokenAsync("http://my.service.uri/app", "clientId", new Uri("callback://complete"));
            return token;
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
