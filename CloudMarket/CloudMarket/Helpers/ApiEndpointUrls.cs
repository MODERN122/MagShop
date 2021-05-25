using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMarket.Helpers
{
    public static class ApiEndpointUrls
    {
        private const string ngrok = "https://698d3eb5da9c.ngrok.io";
        public static string MagShopUrl
        {
            get => ngrok;
        }
    }
}
