using ApplicationCore.Endpoints.Baskets;
using ApplicationCore.Entities;
using Ardalis.GuardClauses;
using CloudMarket.Services;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CloudMarket.ViewModels
{
    public class BasketPageViewModel : BindableBase
    {
        private CancellationToken cancellationToken;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public BasketPageViewModel(
            INavigationService navigationService,
            DataStoreService dataStoreService)
        {
            _navigationService = navigationService;
            _dataStoreService = dataStoreService;

            BasketItems = new ObservableCollection<BasketItemResponse>();
            LoadBasketItemsCommand = new Command(async () => await ExecuteLoadBasketItemsCommand());
            cancellationToken = new CancellationToken();
        }

        private async Task ExecuteLoadBasketItemsCommand()
        {
            IsBusy = true;
            try
            {
                var basketItems = await _dataStoreService.GetBasketItems(cancellationToken);
                Guard.Against.Null(basketItems, nameof(basketItems));
                BasketItems.Clear();
                basketItems.ForEach(x => BasketItems.Add(x));
            }
            catch (Exception)
            {

            }
            IsBusy = false;
        }

        private INavigationService _navigationService;
        private DataStoreService _dataStoreService;
        private bool _isBusy;

        public ObservableCollection<BasketItemResponse> BasketItems { get; private set; }
        public Command LoadBasketItemsCommand { get; }
    }
}