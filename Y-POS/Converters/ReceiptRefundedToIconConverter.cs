using System;
using System.Globalization;
using System.Windows.Data;

namespace Y_POS.Converters
{
    internal class ReceiptRefundedToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isRefunded = (bool) value;

            return isRefunded ? "/Images/refund.png" : "/Images/paid.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
