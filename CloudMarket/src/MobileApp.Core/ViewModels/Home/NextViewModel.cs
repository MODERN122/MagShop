using System.Windows.Input;
using MobileApp.Core.ViewModels.Main;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace MobileApp.Core
{
    public class NextViewModel: MvxViewModel
    {
        private IMvxNavigationService _navigationService;
        private ICommand _submitCommand;
        public ICommand SubmitCommand
        {
            get
            {
                _submitCommand = _submitCommand ?? new MvxCommand(SomeMethodAsync);
                return _submitCommand;
            }
        }
        public NextViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public async void SomeMethodAsync()
        {
            await _navigationService.Navigate<MainViewModel>();
        }
    }
}
