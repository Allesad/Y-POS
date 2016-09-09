using System;
using System.Globalization;
using System.Windows.Data;
using YumaPos.Shared.API.Enums;

namespace Y_POS.Converters
{
    internal class OrderStatusToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (OrderStatus) value;

            switch (status)
            {
                case OrderStatus.Draft:
                    return Core.Properties.Resources.OrderStatus_Draft;
                case OrderStatus.New:
                    return Core.Properties.Resources.OrderStatus_Draft;
                case OrderStatus.OnHold:
                    return Core.Properties.Resources.OrderStatus_OnHold;
                case OrderStatus.Changed:
                    return Core.Properties.Resources.OrderStatus_Changed;
                case OrderStatus.Confirmed:
                    return Core.Properties.Resources.OrderStatus_Confirmed;
                case OrderStatus.InProgress:
                    return Core.Properties.Resources.OrderStatus_InProgress;
                case OrderStatus.Prepared:
                    return Core.Properties.Resources.OrderStatus_Prepared;
                case OrderStatus.AssignedToDriver:
                    return Core.Properties.Resources.OrderStatus_AssignedToDrivder;
                case OrderStatus.OnDelivery:
                    return Core.Properties.Resources.OrderStatus_OnDelivery;
                case OrderStatus.Delivered:
                    return Core.Properties.Resources.OrderStatus_Delivered;
                case OrderStatus.Closed:
                    return Core.Properties.Resources.OrderStatus_Closed;
                case OrderStatus.Void:
                    return Core.Properties.Resources.OrderStatus_Void;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
