using System;
using System.Globalization;
using System.Windows.Data;

namespace Y_POS.Converters
{
    internal class CheckoutCustomerNameToTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var name = value as string;

            return name ?? Core.Properties.Resources.AddCustomer;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
