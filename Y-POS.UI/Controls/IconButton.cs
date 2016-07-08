
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Y_POS.UI.Controls
{
    [TemplatePart( Name = "PART_ICON")]
    public sealed class IconButton : Button
    {
        #region ICON property

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof (Geometry), typeof (IconButton), new PropertyMetadata(default(Geometry)));

        public Geometry Icon
        {
            get { return (Geometry) GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        #endregion


    }
}
