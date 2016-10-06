using System;
using System.Globalization;
using System.Windows.Data;
using Y_POS.Core.Infrastructure;

namespace Y_POS.Converters
{
    internal class DiscountToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var discountName = value as string;

            return discountName.IsEmpty()
                ? Core.Properties.Resources.SelectDiscount
                : discountName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
