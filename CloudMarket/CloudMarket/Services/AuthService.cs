using CloudMarket.Interfaces;
using CloudMarket.Views;
using Prism.Navigation;
using Prism.Navigation.Xaml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Auth.XamarinForms;
using Xamarin.Forms;

namespace CloudMarket.Services
{
    public class AuthService
    {
        public AuthService(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        TaskCompletionSource<LoginResult> _completionSource;
        private INavigationService _navigationService;

        public async Task Login()
        {
            _completionSource = new TaskCompletionSource<LoginResult>();
            var authpage = new AuthenticatorPage(new OAuth2Authenticator
                (
                clientId: "7b275d42-1bd4-4aa6-90c3-3f49bdd7f601",
                scope: "User.Read",
                authorizeUrl: new Uri("https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize"),
                redirectUrl: new Uri("https://login.live.com/oauth20_desktop.srf"),
                clientSecret: null,
                accessTokenUrl: new Uri("https://login.microsoftonline.com/consumers/oauth2/v2.0/token"
                )
                )
            {
                AllowCancel = true                
            });
            await Application.Current.MainPage.Navigation.PushModalAsync(authpage);
        }

        private void AuthOnError(object arg1, AuthenticatorErrorEventArgs arg2)
        {
            throw new NotImplementedException();
        }

        private void AuthOnCompleted(object arg1, AuthenticatorCompletedEventArgs arg2)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}
