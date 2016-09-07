using System;
using System.Windows;
using System.Windows.Input;
using DialogManagement.Contracts;
using DialogManagement.Core;
using YumaPos.Client.Helpers;
using YumaPos.Client.Navigation.Contracts;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : BaseView, IDialogHost
    {
        public MainView()
        {
            InitializeComponent();

            var dlgManager = ServiceLocator.Resolve<IDialogManager>();
            dlgManager.SetHost(this);
        }

        private void CommandBinding_OnNavigateToPage(object sender, ExecutedRoutedEventArgs e)
        {
            /*var target = e.Parameter as string;
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
                    Page.Content = new ClosedOrdersView();
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
                case "Settings":
                    Page.Content = new SettingsView();
                    break;
            }
            NavMenuContainer.Visibility = Visibility.Collapsed;*/
        }

        public void ShowDialog(IBaseDialog dialog)
        {
            MessageBox.Show(((TextDialogContent) dialog.Content).Message);
        }

        public void HideDialog()
        {
            
        }

        private void OnBrowseBack(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                ServiceLocator.Resolve<INavigator>().Back();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnSwitchMenuState(object sender, ExecutedRoutedEventArgs e)
        {
            NavMenuContainer.Visibility = NavMenuContainer.Visibility == Visibility.Collapsed
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void Page_OnContentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            NavMenuContainer.Visibility = Visibility.Collapsed;
        }
    }
}
