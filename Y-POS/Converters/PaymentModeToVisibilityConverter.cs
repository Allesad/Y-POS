using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Y_POS.Core.ViewModels.PageParts;

namespace Y_POS.Converters
{
    internal class PaymentModeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mode = (PaymentVm.PaymentMode) value;
            var p = (string) parameter;

            switch (mode)
            {
                case PaymentVm.PaymentMode.Default:
                    return p.Equals("default") ? Visibility.Visible : Visibility.Collapsed;
                case PaymentVm.PaymentMode.MultiPaymentOverview:
                    return p.Equals("overview") ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
