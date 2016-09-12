using System;
using System.Globalization;
using System.Windows.Data;
using YumaPos.Client.Common;
using YumaPos.Client.Helpers;
using YumaPos.Shared.API.Enums;

namespace Y_POS.Converters
{
    internal class OrderStatusToTextConverter : IValueConverter
    {
        private static IResourcesService _rs;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (OrderStatus) value;

            _rs = _rs ?? ServiceLocator.Resolve<IResourcesService>();

            return _rs.GetNameOfEnum(status);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
