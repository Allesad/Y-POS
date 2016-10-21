using System;
using System.Globalization;
using System.Windows.Data;
using Y_POS.Core.ViewModels.Pages;

namespace Y_POS.Converters
{
    internal class CashierByToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (CashierBy) value;
            var self = (CashierBy) parameter;

            return type == self;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var self = (CashierBy) parameter;

            return self;
        }
    }
}
