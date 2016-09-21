using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace Y_POS.Converters
{
    internal class PhoneFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var phone = value as string;
            if (string.IsNullOrEmpty(phone)) return "-";

            switch (phone.Length)
            {
                case 7:
                    return Regex.Replace(phone, @"(\d{3})(\d{4})", "$1-$2");
                case 10:
                    return Regex.Replace(phone, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
                case 11:
                    return Regex.Replace(phone, @"(\d{1})(\d{3})(\d{3})(\d{4})", "$1-$2-$3-$4");
                default:
                    return phone;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
