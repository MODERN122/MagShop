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

namespace CloudMarket.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private CancellationToken cancellationToken;
        private int _itemSelectedIndex = -1;
        public int ItemSelectedIndex
        {
            get => _itemSelectedIndex;
            set => SetProperty(ref _itemSelectedIndex, value);
        }
        public ObservableCollection<ProductPreview> Items { get; set; }
        public ObservableCollection<Property> Properties { get; set; } = new ObservableCollection<Property>();
        public ObservableCollection<Property> FilteredProperties { get; set; } = new ObservableCollection<Property>();
        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
        public Command LoadItemsCommand { get; set; }
        public Command AddPropertyFilterCommand { get; set; }

        private Category _defaultCategory = new Category { CategoryId = "", Name = "Нет" };

        public ItemsViewModel()
        {
            Title = "Main";
            Items = new ObservableCollection<ProductPreview>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddPropertyFilterCommand = new Command(AddPropertyFilter);
            cancellationToken = new CancellationToken();
        }

        private void AddPropertyFilter()
        {

        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            try
            {
                var res1 = await DataStore.LoginUsernameAsync("demoseller@microsoft.com", "p@SSw0rd", cancellationToken);

                var res3 = await DataStore.GetListProductsAsync(
                    new ApplicationCore.Endpoints.Products.GetProductsRequest()
                    {
                        CategoryId = ItemSelectedIndex!=-1
                                        ? Categories[ItemSelectedIndex].CategoryId : "",
                        PropertiesId = Properties.Select(x=>x.Id).ToList(),
                        PageSize = 6,
                        PageIndex = 0
                    },
                    cancellationToken);
                Categories.Clear();
                Categories.Add(_defaultCategory);
                res3.ForEach(x =>
                {
                    if (!Categories.Any(y => y.CategoryId == x.CategoryId))
                    {
                        Categories.Add(x.Category);
                    }
                    x.Properties.ForEach(z =>
                    {
                        if (!Properties.Select(r => r.Id).Contains(z.Id))
                        {
                            Properties.Add(z);
                        }
                    });
                });
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