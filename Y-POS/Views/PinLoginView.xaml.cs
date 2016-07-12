using System.Windows;
using System.Windows.Controls;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for PinLoginView.xaml
    /// </summary>
    public partial class PinLoginView : UserControl
    {
        public PinLoginView()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ((Window)Window.GetWindow(this)).Content = new LoginView();
        }

        private void ButtonBase_OnClick1(object sender, RoutedEventArgs e)
        {
            ((Window)Window.GetWindow(this)).Content = new ActiveOrdersView();
        }
    }
}
