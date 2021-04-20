using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMarket.Helpers
{
    public static class ApiEndpointUrls
    {
        private const string ngrok = "https://cc57c0590682.ngrok.io";
        public static string MagShopUrl
        {
            get => ngrok;
        }
    }
}
