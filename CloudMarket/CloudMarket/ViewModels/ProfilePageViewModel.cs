using CloudMarket.Interfaces;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMarket.ViewModels
{
    public class ProfilePageViewModel : BindableBase, INavigatedAware
    {
        private IProfileService _profileService;
        private IPageDialogService _pageDialogService;

        public ProfilePageViewModel(
            IProfileService profileService,
            IPageDialogService pageDialogService)
        {
            _profileService = profileService;
            _pageDialogService = pageDialogService;
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
        }
    }
}
