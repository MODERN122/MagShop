﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CloudMarket.Models
{
    public enum MenuItemType
    {
        Browse,
        Basket,
        Login
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
