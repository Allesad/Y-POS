using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace YumaPos.WPF.UI.Controls
{
    public class RepeatIconButton : RepeatButton
    {
        #region IconPath

        public static readonly DependencyProperty IconPathProperty = DependencyProperty.Register(
            "IconPath", typeof(Geometry), typeof(RepeatIconButton), new PropertyMetadata(default(Geometry)));

        public Geometry IconPath
        {
            get { return (Geometry)GetValue(IconPathProperty); }
            set { SetValue(IconPathProperty, value); }
        }

        #endregion

        #region IconWidth

        public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register(
            "IconWidth", typeof(double), typeof(RepeatIconButton), new FrameworkPropertyMetadata(24d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }

        #endregion

        #region IconHeight

        public static readonly DependencyProperty IconHeightProperty = DependencyProperty.Register(
            "IconHeight", typeof(double), typeof(RepeatIconButton), new FrameworkPropertyMetadata(24d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }

        #endregion
    }
}
