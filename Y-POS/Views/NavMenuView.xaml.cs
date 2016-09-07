using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YumaPos.Client.Navigation;
using Y_POS.Core;
using Y_POS.Core.ViewModels;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for NavMenuView.xaml
    /// </summary>
    public partial class NavMenuView : UserControl
    {
        public NavMenuView()
        {
            InitializeComponent();
        }

        private INavMenuVm ViewModel
        {
            get { return (INavMenuVm) DataContext; }
        }

        private void NavMenuListView_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (NavMenuListView.SelectedItem == null) return;

            switch (NavMenuListView.SelectedIndex)
            {
                case 0:
                    NavigationCommands.GoToPage.Execute("ActiveOrders", (IInputElement) e.Source);
                    break;
                case 1:
                    NavigationCommands.GoToPage.Execute("ClosedOrders", (IInputElement) e.Source);
                    break;
                case 2:
                    NavigationCommands.GoToPage.Execute("Cashdrawer", (IInputElement) e.Source);
                    break;
                case 3:
                    NavigationCommands.GoToPage.Execute("Reports", (IInputElement) e.Source);
                    break;
                case 4:
                    NavigationCommands.GoToPage.Execute("Settings", (IInputElement) e.Source);
                    break;
                case 5:
                    NavigationCommands.GoToPage.Execute("PinLogin", (IInputElement) e.Source);
                    break;
            }
        }

        private void NavMenuListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NavMenuListView.SelectedItem == null) return;

            NavUri targetUri = null;
            switch (NavMenuListView.SelectedIndex)
            {
                case 0:
                    targetUri = AppNavigation.ActiveOrders;
                    break;
                case 1:
                    targetUri = AppNavigation.ClosedOrders;
                    break;
                case 2:
                    targetUri = AppNavigation.Cashdrawer;
                    break;
                case 3:
                    targetUri = AppNavigation.Reports;
                    break;
                case 4:
                    targetUri = AppNavigation.Settings;
                    break;
                case 5:
                    targetUri = AppNavigation.PinLogin;
                    break;
            }
            ViewModel.CommandNavigate.Execute(targetUri);
        }
    }
}
