﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.UrlConfiguration
{
    public class BaseUrlConfiguration
    {
        public const string CONFIG_NAME = "baseUrls";

        public string ApiBase { get; set; }
        public string WebBase { get; set; }
    }
}
