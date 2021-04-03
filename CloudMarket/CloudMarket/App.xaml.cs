using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CloudMarket.Services;
using CloudMarket.Views;
using Prism.DryIoc;
using Prism.Ioc;
using CloudMarket.ViewModels;
using System.Diagnostics;

namespace CloudMarket
{
    public partial class App : PrismApplication
    {

        public App()
        {
        }
        protected async override void OnInitialized()
        {
            InitializeComponent();
            var result = await NavigationService.NavigateAsync("MainPage/NavigationPage/BasketPage");
            if (!result.Success)
            {
                Debugger.Break();
            }
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<ItemsPage, ItemsPageViewModel>();
            containerRegistry.RegisterForNavigation<SelectedItemPage, SelectedItemPageViewModel>();
            containerRegistry.RegisterForNavigation<BasketPage, BasketPageViewModel>();

            containerRegistry.RegisterScoped<DataStoreService>();
            containerRegistry.RegisterScoped<AuthService>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
