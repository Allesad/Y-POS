using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Y_POS.UI.Converters
{
    public class TextCaseCoverter : IValueConverter
    {
        public CharacterCasing Case { get; set; }

        public TextCaseCoverter()
        {
            Case = CharacterCasing.Upper;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            if (str == null) return value;

            switch (Case)
            {
                case CharacterCasing.Upper:
                    return str.ToUpper(culture);
                case CharacterCasing.Lower:
                    return str.ToLower(culture);
                default:
                    return str;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
