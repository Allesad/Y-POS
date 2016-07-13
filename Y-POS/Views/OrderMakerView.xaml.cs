using System.Windows;
using System.Windows.Controls;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for OrderMakerView.xaml
    /// </summary>
    public partial class OrderMakerView : UserControl
    {
        public OrderMakerView()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ((Window)Window.GetWindow(this)).Content = new ActiveOrdersView();
        }
    }
}
