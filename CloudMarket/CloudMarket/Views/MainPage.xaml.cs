using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CloudMarket.Models;
using CloudMarket.Services;
using Xamarin.Auth.XamarinForms;
using Xamarin.Auth;
using System.Net.Http;

namespace CloudMarket.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.Browse, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
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
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Browse:
                        MenuPages.Add(id, new NavigationPage(new ItemsPage()));
                        break;
                    case (int)MenuItemType.Basket:
                        MenuPages.Add(id, new NavigationPage(new BasketPage()));
                        break;
                    case (int)MenuItemType.Login:
                        MenuPages.Add(id, new NavigationPage(authPage));
                        break;
                }
                authPage.Authenticator.Completed += Authenticator_Completed;
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }

        private void Authenticator_Completed(object sender, AuthenticatorCompletedEventArgs e)
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
    }
}