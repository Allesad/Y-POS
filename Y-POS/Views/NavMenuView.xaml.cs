using System.Windows.Controls;
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
