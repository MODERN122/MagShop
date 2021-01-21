using ApplicationCore.Endpoints.Baskets;
using ApplicationCore.Entities;
using Ardalis.GuardClauses;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CloudMarket.ViewModels
{
    public class BasketViewModel : BaseViewModel
    {
        private CancellationToken cancellationToken;
        public BasketViewModel()
        {
            Title = "Basket";
            BasketItems = new ObservableCollection<BasketItemResponse>();
            LoadBasketItemsCommand = new Command(async () => await ExecuteLoadBasketItemsCommand());
            cancellationToken = new CancellationToken();
        }

        private async Task ExecuteLoadBasketItemsCommand()
        {
            IsBusy = true;
            try
            {
                var basketItems = await DataStore.GetBasketItems(cancellationToken);
                Guard.Against.Null(basketItems, nameof(basketItems));
                BasketItems.Clear();
                basketItems.ForEach(x => BasketItems.Add(x));
            }
            catch (Exception)
            {

            }
            IsBusy = false;
        }

        public ObservableCollection<BasketItemResponse> BasketItems { get; private set; }
        public Command LoadBasketItemsCommand { get; }
    }
}