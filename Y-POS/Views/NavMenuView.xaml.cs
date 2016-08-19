using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void NavMenuListView_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (NavMenuListView.SelectedItem == null) return;

            switch (NavMenuListView.SelectedIndex)
            {
                case 0:
                    NavigationCommands.GoToPage.Execute("ActiveOrders", (IInputElement) e.Source);
                    break;
                case 1:
                    NavigationCommands.GoToPage.Execute("ActiveOrders", (IInputElement) e.Source);
                    break;
                case 2:
                    NavigationCommands.GoToPage.Execute("Cashdrawer", (IInputElement) e.Source);
                    break;
                case 3:
                    NavigationCommands.GoToPage.Execute("Reports", (IInputElement) e.Source);
                    break;
            }
        }
    }
}
