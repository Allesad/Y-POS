using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using YumaPos.Shared.API.Enums;

namespace Y_POS.Converters
{
    internal class SexToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var gender = (Gender?) value;

            switch (gender)
            {
                case Gender.Male:
                    return Core.Properties.Resources.Sex_Male;
                case Gender.Female:
                    return Core.Properties.Resources.Sex_Female;
                default:
                    return "-";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string genderString = null;

            var cbItem = value as ComboBoxItem;
            if (cbItem != null)
            {
                genderString = (string) cbItem.Content;
            }

            if (string.IsNullOrEmpty(genderString)) return null;

            if (genderString.Equals(Core.Properties.Resources.Sex_Male))
            {
                return Gender.Male;
            }
            if (genderString.Equals(Core.Properties.Resources.Sex_Female))
            {
                return Gender.Female;
            }
            return null;
        }
    }
}
