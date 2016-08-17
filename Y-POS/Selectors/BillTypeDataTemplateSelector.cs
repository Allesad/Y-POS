using System.Windows;
using System.Windows.Controls;

namespace Y_POS.Selectors
{
    public sealed class BillTypeDataTemplateSelector : DataTemplateSelector
    {
        #region Fields

        /*private DataTemplate _defaultTemplate;
        private DataTemplate _coinsTemplate;*/

        #endregion

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

            if (billItem.IsCoins)
            {
                /*_coinsTemplate = (DataTemplate) (_coinsTemplate ?? element.FindResource("CoinsTypeDt"));
                return _coinsTemplate;*/
                return CoinsTemplate;
            }

            /*_defaultTemplate = (DataTemplate) (_defaultTemplate ?? element.FindResource("BillTypeDt"));
            return _defaultTemplate;*/
            return DefaultTemplate;
        }
    }
}
