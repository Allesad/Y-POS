using System;
using System.Globalization;
using System.Windows.Data;
using YumaPos.Shared.API.Enums;

namespace Y_POS.Converters
{
    internal class SplittingTypeToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (SplittingType) value;

            switch (type)
            {
                case SplittingType.AllOnOne:
                    return Core.Properties.Resources.Split_AllOnOne;
                case SplittingType.SplitEvently:
                    return Core.Properties.Resources.Split_Evenly;
                case SplittingType.SplitProportionally:
                    return Core.Properties.Resources.Split_Proportionally;
                case SplittingType.SplitBySeating:
                    return Core.Properties.Resources.Split_BySeats;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
