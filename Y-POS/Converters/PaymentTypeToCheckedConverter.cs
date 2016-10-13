using System;
using System.Globalization;
using System.Windows.Data;
using Y_POS.Core.Enums;

namespace Y_POS.Converters
{
    internal class PaymentTypeToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (PaymentType) value;
            var controlType = (PaymentType) parameter;

            return type == controlType;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
