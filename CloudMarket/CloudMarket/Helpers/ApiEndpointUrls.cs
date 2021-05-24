using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMarket.Helpers
{
    public static class ApiEndpointUrls
    {
        private const string ngrok = "https://7a18206a8842.ngrok.io";
        public static string MagShopUrl
        {
            get => ngrok;
        }
    }
}
