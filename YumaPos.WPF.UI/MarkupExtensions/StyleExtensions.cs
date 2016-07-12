using System.Windows;
using System.Windows.Media;

namespace YumaPos.WPF.UI.MarkupExtensions
{
    public static class StyleExtensions
    {
        #region Background on hover

        public static readonly DependencyProperty BgOnHoverProperty = DependencyProperty.RegisterAttached(
            "BgOnHover", typeof(Brush), typeof(StyleExtensions),
            new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetBgOnHover(DependencyObject element, Brush value)
        {
            element.SetValue(BgOnHoverProperty, value);
        }

        public static Brush GetBgOnHover(DependencyObject element)
        {
            return (Brush)element.GetValue(BgOnHoverProperty);
        }

        #endregion

        #region Background on pressed

        public static readonly DependencyProperty BgOnPressedProperty = DependencyProperty.RegisterAttached(
            "BgOnPressed", typeof(Brush), typeof(StyleExtensions),
            new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetBgOnPressed(DependencyObject element, Brush value)
        {
            element.SetValue(BgOnPressedProperty, value);
        }

        public static Brush GetBgOnPressed(DependencyObject element)
        {
            return (Brush)element.GetValue(BgOnPressedProperty);
        }

        #endregion

        #region Background on checked

        public static readonly DependencyProperty BgOnCheckedProperty = DependencyProperty.RegisterAttached(
            "BgOnChecked", typeof(Brush), typeof(StyleExtensions), new PropertyMetadata(default(Brush)));

        public static void SetBgOnChecked(DependencyObject element, Brush value)
        {
            element.SetValue(BgOnCheckedProperty, value);
        }

        public static Brush GetBgOnChecked(DependencyObject element)
        {
            return (Brush)element.GetValue(BgOnCheckedProperty);
        }

        #endregion

        #region Foreground on hover

        public static readonly DependencyProperty ForegroundOnHoverProperty = DependencyProperty.RegisterAttached(
            "ForegroundOnHover", typeof(Brush), typeof(StyleExtensions),
            new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetForegroundOnHover(DependencyObject element, Brush value)
        {
            element.SetValue(ForegroundOnHoverProperty, value);
        }

        public static Brush GetForegroundOnHover(DependencyObject element)
        {
            return (Brush)element.GetValue(ForegroundOnHoverProperty);
        }

        #endregion

        #region Foreground on pressed

        public static readonly DependencyProperty ForegroundOnPressedProperty = DependencyProperty.RegisterAttached(
            "ForegroundOnPressed", typeof(Brush), typeof(StyleExtensions),
            new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetForegroundOnPressed(DependencyObject element, Brush value)
        {
            element.SetValue(ForegroundOnPressedProperty, value);
        }

        public static Brush GetForegroundOnPressed(DependencyObject element)
        {
            return (Brush)element.GetValue(ForegroundOnPressedProperty);
        }

        #endregion

        #region Foreground on checked

        public static readonly DependencyProperty ForegroundOnCheckedProperty = DependencyProperty.RegisterAttached(
            "ForegroundOnChecked", typeof(Brush), typeof(StyleExtensions), new PropertyMetadata(default(Brush)));

        public static void SetForegroundOnChecked(DependencyObject element, Brush value)
        {
            element.SetValue(ForegroundOnCheckedProperty, value);
        }

        public static Brush GetForegroundOnChecked(DependencyObject element)
        {
            return (Brush)element.GetValue(ForegroundOnCheckedProperty);
        }

        #endregion

        #region Border brush on hover

        public static readonly DependencyProperty BorderBrushOnHoverProperty = DependencyProperty.RegisterAttached(
            "BorderBrushOnHover", typeof(Brush), typeof(StyleExtensions),
            new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetBorderBrushOnHover(DependencyObject element, Brush value)
        {
            element.SetValue(BorderBrushOnHoverProperty, value);
        }

        public static Brush GetBorderBrushOnHover(DependencyObject element)
        {
            return (Brush)element.GetValue(BorderBrushOnHoverProperty);
        }

        #endregion

        #region Border brush on pressed

        public static readonly DependencyProperty BorderBrushOnPressedProperty = DependencyProperty.RegisterAttached(
            "BorderBrushOnPressed", typeof(Brush), typeof(StyleExtensions),
            new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetBorderBrushOnPressed(DependencyObject element, Brush value)
        {
            element.SetValue(BorderBrushOnPressedProperty, value);
        }

        public static Brush GetBorderBrushOnPressed(DependencyObject element)
        {
            return (Brush)element.GetValue(BorderBrushOnPressedProperty);
        }

        #endregion

        #region Corner Radius

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius", typeof(CornerRadius), typeof(StyleExtensions), new PropertyMetadata(default(CornerRadius)));

        public static void SetCornerRadius(DependencyObject element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }

        public static CornerRadius GetCornerRadius(DependencyObject element)
        {
            return (CornerRadius)element.GetValue(CornerRadiusProperty);
        }

        #endregion
    }
}
