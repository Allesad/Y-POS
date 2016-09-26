using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Y_POS.Converters
{
    internal class CustomerControlLayoutConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isNarrow = (bool) value;

            return isNarrow ? Dock.Top : Dock.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
