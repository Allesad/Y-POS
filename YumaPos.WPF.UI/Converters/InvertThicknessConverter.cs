using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace YumaPos.WPF.UI.Converters
{
    public class InvertThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness) return InvertThickness((Thickness)value);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness) return InvertThickness((Thickness)value);
            return value;
        }

        private static Thickness InvertThickness(Thickness value)
        {
            return new Thickness(-value.Left, -value.Top, -value.Right, -value.Bottom);
        }
    }
}
