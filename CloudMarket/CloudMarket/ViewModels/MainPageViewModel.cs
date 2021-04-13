using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Auth.XamarinForms;
using Xamarin.Forms;

namespace CloudMarket.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        public MainPageViewModel(
            INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateCommand = new Command<string>(NavigateCommandExecuted);
        }

        private INavigationService _navigationService;

        public Command<string> NavigateCommand { get; }

        private async void NavigateCommandExecuted(string path)
        {
            var result = await _navigationService.NavigateAsync($"NavigationPage/{path}");
            if (!result.Success)
            {
                Debugger.Break();
            }
        }

        public async Task OAuthAsync()
        {
            var authPage = new AuthenticatorPage(new OAuth2Authenticator
                (
                clientId: "7b275d42-1bd4-4aa6-90c3-3f49bdd7f601",
                scope: "User.Read, User.ReadBasic.All",
                authorizeUrl: new Uri("https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize"),
                redirectUrl: new Uri("https://login.microsoftonline.com/common/oauth2/nativeclient"),
                clientSecret: null,
                accessTokenUrl: new Uri("https://login.microsoftonline.com/consumers/oauth2/v2.0/token")
                )
            {
                AllowCancel = true
            });
            authPage.Authenticator.Completed += Authenticator_Completed;
        }

        private void Authenticator_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            try
            {
                var username = e.Account.Username;
                if (sender is OAuth2Authenticator oAuth2)
                {
                    var url = oAuth2.AuthorizeUrl;
                }
                var token = e.Account.Properties["access_token"];
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = new HttpResponseMessage();
                Task.Run(async () => {
                    response = await client.GetAsync("https://graph.microsoft.com/v1.0/me");
                    if (response.IsSuccessStatusCode)
                    {

                    }
                });
            }
            catch (Exception ex)
            {

            }
        }
    }
}
