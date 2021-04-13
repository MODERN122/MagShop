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
using System.Linq;
using Xamarin.Forms.Internals;
using System.Collections.Generic;
using Prism.Mvvm;
using CloudMarket.Services;
using Prism.Navigation;

namespace CloudMarket.ViewModels
{
    public class ItemsPageViewModel : BindableBase
    {
        private CancellationToken cancellationToken;
        private int _itemSelectedIndex = -1;
        public int ItemSelectedIndex
        {
            get => _itemSelectedIndex;
            set => SetProperty(ref _itemSelectedIndex, value);
        }

        private INavigationService _navigationService;
        private AuthService _authService;
        private DataStoreService _dataStoreService;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ObservableCollection<ProductPreview> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }
        public ObservableCollection<Property> UnfilteredProperties { get; set; } = new ObservableCollection<Property>();
        public ObservableCollection<Property> FilteredProperties { get; set; } = new ObservableCollection<Property>();
        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
        public Command LoadItemsCommand { get; set; }
        public Command AddPropertyFilterCommand { get; set; }
        public Command RemovePropertyFilterCommand { get; }
        public Command AuthByMicrosoftCommand { get; }

        private Category _defaultCategory = new Category { CategoryId = "", Name = "Нет" };
        private ObservableCollection<ProductPreview> _items = new ObservableCollection<ProductPreview>()
        {
            new ProductPreview()
            {
                ProductId = "dasdadad",
                ProductName = "productName",
                PriceNew = 1000.0,
            }
        };
        private bool _isBusy;

        public ItemsPageViewModel(
            INavigationService navigationService,
            AuthService authService,
            DataStoreService dataStoreService)
        {
            _navigationService = navigationService;
            _authService = authService;
            _dataStoreService = dataStoreService;

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddPropertyFilterCommand = new Command(AddPropertyFilter);
            RemovePropertyFilterCommand = new Command(RemovePropertyFilter);
            AuthByMicrosoftCommand = new Command(AuthByMicrosoft);
            cancellationToken = new CancellationToken();
        }

        private async void AuthByMicrosoft(object obj)
        {
            await _authService.Login();
        }

        private void RemovePropertyFilter(object obj)
        {
            var property = FilteredProperties.First(x => x == (Property)obj);
            FilteredProperties.Remove(property);
            UnfilteredProperties.Add(property);
        }

        private void AddPropertyFilter(object obj)
        {
            var property = UnfilteredProperties.First(x => x == (Property)obj);
            UnfilteredProperties.Remove(property);
            FilteredProperties.Add(property);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            try
            {
                var res1 = await _dataStoreService.LoginUsernameAsync("demoseller@microsoft.com", "p@SSw0rd", cancellationToken);
                if (res1)
                {

                }
                var res3 = await _dataStoreService.GetListProductsAsync(
                    new ApplicationCore.Endpoints.Products.GetProductsRequest()
                    {
                        CategoryId = ItemSelectedIndex != -1
                                        ? Categories[ItemSelectedIndex].CategoryId : "",
                        PropertiesId = FilteredProperties.Count != 0
                            ? FilteredProperties.Select(x => x.PropertyName).ToList()
                            : null,
                        PageSize = 20,
                        PageIndex = 0
                    },
                    cancellationToken);
                Categories.Clear();
                UnfilteredProperties.Clear();
                Categories.Add(_defaultCategory);
                if (res3 != null)
                {
                    res3.ForEach(x =>
                    {
                        if (!Categories.Any(y => y.CategoryId == x.CategoryId))
                        {
                            Categories.Add(x.Category);
                        }
                        x.Properties.ForEach(z =>
                        {
                            if (!UnfilteredProperties.Select(r => r.PropertyName).Contains(z.PropertyName))
                            {
                                UnfilteredProperties.Add(z);
                            }
                        });
                        if (x.Properties[0]?.PropertyItems[2]?.Image != null && x.PreviewImage==null)
                        {
                            x.PreviewImage = x.Properties[0]?.PropertyItems[2]?.Image;
                        }
                    });
                    Items.Clear();
                    res3.ForEach(x => Items.Add(x));
                }
                Items.Add(new ProductPreview()
                {
                    ProductId = "dasdadad",
                    ProductName = "productName",
                    PriceNew = 1000.0,
                });
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