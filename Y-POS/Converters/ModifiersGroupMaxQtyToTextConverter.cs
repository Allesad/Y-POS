using System;
using System.Globalization;
using System.Windows.Data;

namespace Y_POS.Converters
{
    internal class ModifiersGroupMaxQtyToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int qty = (int) value;

            return qty > 0 && qty < int.MaxValue ? qty.ToString() : "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
