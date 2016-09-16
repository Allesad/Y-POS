using System.Windows;
using System.Windows.Controls;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Selectors
{
    internal class SelectedModifiersListTemplateSelector : DataTemplateSelector
    {
        #region Fields

        private DataTemplate _singleQtyDt;
        private DataTemplate _multiQtyDt;

        #endregion

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var modifier = (IModifierItemVm) item;

            var element = (FrameworkElement) container;

            if (modifier.GroupMaxQty > 1)
            {
                _multiQtyDt = (DataTemplate) (_multiQtyDt ?? element.FindResource("MultiQtyModifierDt"));
                return _multiQtyDt;
            }

            _singleQtyDt = (DataTemplate) (_singleQtyDt ?? element.FindResource("SingleChoiceModifierDt"));
            return _singleQtyDt;
        }
    }
}
