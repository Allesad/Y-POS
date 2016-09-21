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
            string size = parameter as string;

            size = size ?? "100x100";
            
            return model?.ImageUri?.AbsoluteUri == null
                ? null 
                : new Uri(model.ImageUri.AbsoluteUri.Replace("$size$", size));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
