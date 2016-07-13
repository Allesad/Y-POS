using System.Windows;
using System.Windows.Controls;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for ActiveOrdersView.xaml
    /// </summary>
    public partial class ActiveOrdersView : UserControl
    {
        public ActiveOrdersView()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            NewOrderBtn.IsEnabled = !NewOrderBtn.IsEnabled;
        }

        private void NewOrderBtn_OnClick(object sender, RoutedEventArgs e)
        {
            ((Window)Window.GetWindow(this)).Content = new OrderMakerView();
        }
    }
}
