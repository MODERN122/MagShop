using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMarket.Helpers
{
    public static class ApiEndpointUrls
    {
        private const string ngrok = "https://ea5501b7b096.ngrok.io";

        public static string MagShopUrl
        {
#if DEBUG
            get => ngrok;
#else
            get=> "http://34.77.154.253";
#endif
        }
    }
}
