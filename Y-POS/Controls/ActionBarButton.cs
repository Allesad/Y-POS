using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Y_POS.Controls
{
    internal class ActionBarButton : Button
    {
        #region IconPath property

        public static readonly DependencyProperty IconPathProperty = DependencyProperty.Register(
            "IconPath", typeof (Geometry), typeof (ActionBarButton),
            new PropertyMetadata(default(Geometry)));

        public Geometry IconPath
        {
            get { return (Geometry) GetValue(IconPathProperty); }
            set { SetValue(IconPathProperty, value); }
        }

        #endregion

        #region Title

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof (string), typeof (ActionBarButton),
            new PropertyMetadata(default(string)));

        public string Title
        {
            get { return (string) GetValue(IconPathProperty); }
            set { SetValue(IconPathProperty, value); }
        }

        #endregion
    }
}