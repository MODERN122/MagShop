using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using CloudMarket.Models;
using CloudMarket.Views;
using Refit;
using CloudMarket.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Polly;
using System.Net;

namespace CloudMarket.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private CancellationToken cancellationToken;

        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
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
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                HttpClient httpClient = new HttpClient(new AuthenticatedHttpClientHandler(GetToken));
                httpClient.BaseAddress = new Uri("https://4de3e90d19e1.ngrok.io");
                httpClient.Timeout = TimeSpan.FromSeconds(10);

                var apiResponse = RestService.For<IMagShopApi>(httpClient);
                var response = await MakeRequest(ctx=>  apiResponse.AuthenticateAsync(new AuthenticationRequest() { Username = "demoseller@microsoft.com", Password = "p@SSw0rd" }, ctx), cancellationToken);
                
                if (response != null)
                {

                }
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}