using System.Windows;
using System.Windows.Controls;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for ClosedOrdersView.xaml
    /// </summary>
    public partial class ClosedOrdersView : UserControl
    {
        public ClosedOrdersView()
        {
            InitializeComponent();
        }

        private void OnMenuClick(object sender, RoutedEventArgs e)
        {
            MainView.SwitchMenuState();
        }
    }
}
