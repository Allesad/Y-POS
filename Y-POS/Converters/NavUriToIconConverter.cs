using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using YumaPos.Client.Navigation;
using Y_POS.Core;

namespace Y_POS.Converters
{
    internal class NavUriToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var uri = (NavUri) value;

            if (uri.Equals(AppNavigation.ActiveOrders)) return Application.Current.FindResource("ActiveListIcon");
            if (uri.Equals(AppNavigation.ClosedOrders)) return Application.Current.FindResource("ClosedListIcon");
            if (uri.Equals(AppNavigation.Cashdrawer)) return Application.Current.FindResource("CurrencyIcon");
            if (uri.Equals(AppNavigation.Reports)) return Application.Current.FindResource("PerformanceIcon");
            if (uri.Equals(AppNavigation.Settings)) return Application.Current.FindResource("SettingsIcon");
            if (uri.Equals(AppNavigation.PinLogin)) return Application.Current.FindResource("QuitIcon");
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
