using System;
using System.Globalization;
using System.Windows.Data;
using Y_POS.Core.ViewModels.Pages;

namespace Y_POS.Converters
{
    internal class CashierStateToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = (CashdrawerState) value;
            var self = (CashdrawerState) parameter;

            return state == self;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var self = (CashdrawerState) parameter;

            return self;
        }
    }
}
