using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using CloudMarket.Models;
using CloudMarket.Views;
using Refit;
using CloudMarket.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Polly;
using System.Net;
using ApplicationCore.Entities;

namespace CloudMarket.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private CancellationToken cancellationToken;

        public ObservableCollection<ProductPreview> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<ProductPreview>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                var res1 = await DataStore.LoginUsernameAsync("demoseller@microsoft.com", "p@SSw0rd", new CancellationToken());
                var res2 = await DataStore.RegisterUserAsync(
                    new ApplicationCore.Endpoints.Users.CreateUserRequest()
                    {
                        Password = "daSADA231321@@sdad",
                        Email = "mobile@mail.ru"
                    }, 
                    new CancellationToken());
                var res3 = await DataStore.GetListProductsAsync(
                    new ApplicationCore.Endpoints.Products.GetProductsRequest(),
                    new CancellationToken());
                Items.Clear();
                res3.ForEach(x => Items.Add(x));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}