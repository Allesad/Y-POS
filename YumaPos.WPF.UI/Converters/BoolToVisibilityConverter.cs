using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace YumaPos.WPF.UI.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var invert = parameter != null && bool.Parse(parameter.ToString());
            var visible = value is bool && (bool)value;

            var result = visible ^ invert ? Visibility.Visible : Visibility.Collapsed;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
