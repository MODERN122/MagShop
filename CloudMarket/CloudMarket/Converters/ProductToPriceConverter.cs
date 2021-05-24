using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CloudMarket.Converters
{
    public class ProductToPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is ProductPreview product)
            {
                foreach (var item in product.Properties)
                {
                    var propertyItem = item.PropertyItems.First();
                    if (propertyItem!=null && propertyItem.PriceNew != -1)
                    {
                        return propertyItem.PriceNew;
                    }
                }
            }
            return "NAN";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
