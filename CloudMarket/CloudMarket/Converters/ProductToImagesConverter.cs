using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using Xamarin.Forms;

namespace CloudMarket.Converters
{
    public class ProductToImagesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Product product)
            {
                List<ImageSource> images = new List<ImageSource>();
                images.Add(ImageSource.FromStream(() => new MemoryStream(product.PreviewImage)));
                if(product.Images.Count > 0)
                    images.AddRange(product.Images.Select(x => ImageSource.FromStream(() => new MemoryStream(x.ByteImage))));
                product.Properties.ForEach((x) => x.PropertyItems.ForEach(y =>
                {
                    if (y.Image != null)
                    {
                        images.Add(ImageSource.FromStream(() => new MemoryStream(y.Image)));
                    }
                }));
                return images;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
