using System;
using System.Globalization;
using System.Windows.Data;
using YumaPos.Client.Common;

namespace Y_POS.Converters
{
    internal class ImageModelToSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var model = (IImageModel) value;

            return new Uri(model.ImageUri.AbsoluteUri.Replace("$size$", "128x128"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
