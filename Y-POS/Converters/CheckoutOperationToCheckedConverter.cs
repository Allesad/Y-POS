using System;
using System.Globalization;
using System.Windows.Data;
using Y_POS.Core.ViewModels.Pages;

namespace Y_POS.Converters
{
    internal class CheckoutOperationToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var operation = (CheckoutOperationType) value;
            var controlOperation = (CheckoutOperationType) parameter;

            return operation == controlOperation;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
