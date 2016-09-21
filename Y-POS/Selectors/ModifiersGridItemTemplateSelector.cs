using System.Windows;
using System.Windows.Controls;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Selectors
{
    internal class ModifiersGridItemTemplateSelector : DataTemplateSelector
    {
        #region Fields

        private DataTemplate _defaultTemplate;
        private DataTemplate _skipTemplate;

        #endregion

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var modifier = (IModifierItemVm) item;

            FrameworkElement element = (FrameworkElement) container;

            if (modifier.IsSkipOption)
            {
                _skipTemplate = (DataTemplate) (_skipTemplate ?? element.FindResource("SkipModifierDt"));
                return _skipTemplate;
            }

            _defaultTemplate = (DataTemplate) (_defaultTemplate ?? element.FindResource("ModifierButtonDt"));
            return _defaultTemplate;
        }
    }
}
