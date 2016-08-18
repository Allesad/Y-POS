using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        private static ContentPresenter _container;
        private static Grid _navMenuContainer;

        public MainView()
        {
            InitializeComponent();

            _container = Page;
            _navMenuContainer = NavMenuContainer;
            NavMenuContainer.Visibility = Visibility.Collapsed;
            Page.Content = new LoginView();
        }

        private void CommandBinding_OnNavigateToPage(object sender, ExecutedRoutedEventArgs e)
        {
            var target = e.Parameter as string;
            if (string.IsNullOrEmpty(target)) return;

            switch (target)
            {
                case "Login":
                    Page.Content = new LoginView();
                    break;
                case "PinLogin":
                    Page.Content = new PinLoginView();
                    break;
                case "ActiveOrders":
                    Page.Content = new ActiveOrdersView();
                    break;
                case "ClosedOrders":

                    break;
                case "OrderMaker":
                    Page.Content = new OrderMakerView();
                    break;
                case "Checkout":
                    Page.Content = new CheckoutView();
                    break;
                case "Cashdrawer":
                    Page.Content = new CashDrawerView();
                    break;
                case "Reports":
                    Page.Content = new ReportsView();
                    break;
            }
            NavMenuContainer.Visibility = Visibility.Collapsed;
        }

        public static ContentPresenter GetContainer()
        {
            return _container;
        }

        public static void SwitchMenuState()
        {
            _navMenuContainer.Visibility = _navMenuContainer.Visibility == Visibility.Collapsed 
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public static void HideMenu()
        {
            _navMenuContainer.Visibility = Visibility.Collapsed;
        }
    }
}
