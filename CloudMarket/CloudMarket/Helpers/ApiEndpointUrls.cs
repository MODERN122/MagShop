using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMarket.Helpers
{
    public static class ApiEndpointUrls
    {
        private const string ngrok = "https://264ac4ddfa08.ngrok.io";
        public static string MagShopUrl
        {
            get => ngrok;
        }
    }
}
