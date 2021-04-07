using ApplicationCore.Entities;
using CloudMarket.Interfaces;
using CloudMarket.Services;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CloudMarket.ViewModels
{
    public class ProfilePageViewModel : BindableBase, INavigatedAware
    {
        private ProfileService _profileService;
        private IPageDialogService _pageDialogService;
        private CancellationTokenSource cancellationToken;
        private User _user;

        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public ProfilePageViewModel(
            ProfileService profileService,
            IPageDialogService pageDialogService)
        {
            _profileService = profileService;
            _pageDialogService = pageDialogService;
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            cancellationToken.Cancel();
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            cancellationToken = new CancellationTokenSource();
            User = await _profileService.GetUserAsync(cancellationToken.Token);
            if (User != null)
            {

            }
        }
    }
}
