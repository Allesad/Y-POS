using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using YumaPos.WPF.UI.Converters;

namespace YumaPos.WPF.UI.Helpers
{
    public static class TextCaseHelper
    {
        #region Case property

        public static readonly DependencyProperty TextCaseProperty = DependencyProperty.RegisterAttached(
            "TextCase", typeof (CharacterCasing), typeof (TextCaseHelper),
            new FrameworkPropertyMetadata(CharacterCasing.Normal, FrameworkPropertyMetadataOptions.AffectsRender, OnTextCaseChanged));

        public static void SetTextCase(DependencyObject element, CharacterCasing value)
        {
            element.SetValue(TextCaseProperty, value);
        }

        public static CharacterCasing GetTextCase(DependencyObject element)
        {
            return (CharacterCasing) element.GetValue(TextCaseProperty);
        }

        private static void OnTextCaseChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var tb = obj as TextBlock;
            if (tb == null) return;

            CharacterCasing textCase = (CharacterCasing) e.NewValue;

            var original = tb.GetBindingExpression(TextBlock.TextProperty);

            var b = new Binding
            {
                Source = tb
            };

            if (original != null)
            {
                b.Path = original.ParentBinding.Path;
            }
            else
            {
                b.Source = tb.Text;
            }

            b.Converter = new TextCaseCoverter {Case = textCase};

            tb.SetBinding(TextBlock.TextProperty, b);
        }

        #endregion
    }
}