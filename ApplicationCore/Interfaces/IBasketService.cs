using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IBasketService
    {
        Task TransferBasketAsync(string anonymousId, string userName);
        Task AddItemToBasket(string basketId, int catalogItemId, decimal price, int quantity = 1);
        Task SetQuantities(string basketId, Dictionary<string, int> quantities);
        Task DeleteBasketAsync(string basketId);
    }
}
