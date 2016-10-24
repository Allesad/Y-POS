using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Y_POS.Controls
{
    /// <summary>
    /// Special class for list-like collection of button to switch between operation types in some views.
    /// Contains properties for Icon geometry data and Header text.
    /// </summary>
    internal class OperationSelectionButton : RadioButton
    {
        #region Icon

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof (Geometry), typeof (OperationSelectionButton), new PropertyMetadata(default(Geometry)));

        public Geometry Icon
        {
            get { return (Geometry) GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        #endregion

        #region Header

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof (string), typeof (OperationSelectionButton), new PropertyMetadata(default(string)));

        public string Header
        {
            get { return (string) GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        #endregion
    }
}
