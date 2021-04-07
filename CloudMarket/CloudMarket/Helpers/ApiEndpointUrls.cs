using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMarket.Helpers
{
    public static class ApiEndpointUrls
    {
        private const string ngrock = "https://64578c3994fc.ngrok.io";
        public static string MagShopUrl
        {
            get => ngrock;
        }
    }
}
