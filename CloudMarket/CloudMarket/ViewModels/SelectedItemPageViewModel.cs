﻿using ApplicationCore.Entities;
using CloudMarket.Services;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CloudMarket.ViewModels
{
    public class SelectedItemPageViewModel : BindableBase, INavigatedAware
    {
        private INavigationService _navigationService;
        private DataStoreService _dataStoreService;
        private CancellationTokenSource cancellationToken;
        private Product _product;

        public SelectedItemPageViewModel(
            INavigationService navigationService,
            DataStoreService dataStoreService
            )
        {
            _navigationService = navigationService;
            _dataStoreService = dataStoreService;
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
            }
        }
    }
}