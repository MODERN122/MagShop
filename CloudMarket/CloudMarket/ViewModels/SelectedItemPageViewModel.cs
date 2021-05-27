using ApplicationCore.Entities;
using CloudMarket.Services;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace CloudMarket.ViewModels
{
    public class SelectedItemPageViewModel : BindableBase, INavigatedAware
    {
        private INavigationService _navigationService;
        private DataStoreService _dataStoreService;

        public AsyncCommand OpenLinkCommand { get; }

        private CancellationTokenSource cancellationToken;
        private Product _product;

        public SelectedItemPageViewModel(
            INavigationService navigationService,
            DataStoreService dataStoreService
            )
        {
            _navigationService = navigationService;
            _dataStoreService = dataStoreService;
            OpenLinkCommand = new AsyncCommand(OpenLink);
        }

        private async Task OpenLink()
        {
            await Launcher.OpenAsync(Product.Url);
        }

        public Product Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            cancellationToken.Cancel();
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            cancellationToken = new CancellationTokenSource();
            if (parameters.ContainsKey("id"))
            {
                Product = await _dataStoreService.GetProductAsync((string)parameters["id"], cancellationToken.Token);
                if (Product != null)
                {
                    Product.Images.Add(new Image() { ByteImage = Product.PreviewImage, ProductId = Product.ProductId });
                }
            }
        }
    }
}
