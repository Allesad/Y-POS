using System.Windows;
using System.Windows.Controls;

namespace Y_POS.Selectors
{
    public sealed class BillTypeDataTemplateSelector : DataTemplateSelector
    {
        #region Properties

        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate CoinsTemplate { get; set; }

        #endregion

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            dynamic billItem = item;
            if (billItem == null) return null;

            FrameworkElement element = container as FrameworkElement;
            if (element == null) return null;

            return billItem.IsCoins ? CoinsTemplate : DefaultTemplate;
        }
    }
}
