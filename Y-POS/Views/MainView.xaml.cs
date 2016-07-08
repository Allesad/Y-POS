using System.Windows.Controls;
using Y_POS.Module.Auth.Views;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            Page.Content = new LoginView();
        }
    }
}
