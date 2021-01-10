using System;
using System.Windows.Input;
using MobileApp.Core.ViewModels.Main;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Xamarin.Auth;

namespace MobileApp.Core.ViewModels
{
    public class LoginPageViewModel: MvxViewModel
    {
        private IMvxNavigationService _navigationService;
        private ICommand _facebookAuthCommand;
        private OAuth2Authenticator _authenticator;

        public ICommand FacebookAuthCommand
        {
            get
            {
                _facebookAuthCommand = _facebookAuthCommand ?? new MvxCommand(FacebookAuthAsync);
                return _facebookAuthCommand;
            }
        }
        public LoginPageViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public async void FacebookAuthAsync()
        {
            var facebookAppId = "";
            _authenticator = new OAuth2Authenticator(
                clientId: facebookAppId,
                scope: "email",
                authorizeUrl: new Uri("https://www.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html")
                );
            _authenticator.Completed += Authenticator_Facebook_Completed;
            _authenticator.Error += _authenticator_Error;
            //AuthenticationState.Authenticator = _authenticator;
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(_authenticator);
        }

        private void _authenticator_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Authenticator_Facebook_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            _navigationService.Navigate<NextViewModel>();
        }
    }
}
