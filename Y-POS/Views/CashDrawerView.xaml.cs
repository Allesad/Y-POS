using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Y_POS.Views.CashDrawerParts;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for CashDrawerView.xaml
    /// </summary>
    public partial class CashDrawerView : UserControl
    {
        public CashDrawerView()
        {
            InitializeComponent();

            OperationsList.ItemsSource = new[]
            {
                new OperationItem((Geometry) FindResource("PerformanceIcon"), "PERFORMANCE"),
                new OperationItem((Geometry) FindResource("CashierInIcon"), "CASHIER IN"),
                new OperationItem((Geometry) FindResource("CashdrawerCheckIcon"), "CASHIER CHECK"),
                new OperationItem((Geometry) FindResource("BankIcon"), "BANK WITHDRAW"),
                new OperationItem((Geometry) FindResource("ArrowDownIcon"), "CASH IN"),
                new OperationItem((Geometry) FindResource("ArrowUpIcon"), "CASH OUT")
            };

            OperationsList.SelectedIndex = 1;
        }

        private class OperationItem
        {
            public Geometry Icon { get; set; }
            public string Title { get; set; }

            public OperationItem(Geometry icon, string title)
            {
                Icon = icon;
                Title = title;
            }
        }

        private void OperationsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = (Selector) sender;

            switch (list.SelectedIndex)
            {
                case 0:
                    ContentContainer.Content = new CashDrawerPerformanceView();
                    break;
                case 1:
                    ContentContainer.Content = new CashDrawerInitView();
                    break;
            }
            UpdateActionBar(list.SelectedIndex);
        }

        private void UpdateActionBar(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    MainActionButton.Visibility = Visibility.Collapsed;
                    SendActionButton.Visibility =
                        PrintActionButton.Visibility = FilterActionButton.Visibility = Visibility.Visible;
                    break;
                default:
                    MainActionButton.Visibility = Visibility.Visible;
                    SendActionButton.Visibility =
                        PrintActionButton.Visibility = FilterActionButton.Visibility = Visibility.Collapsed;
                    break;
            }
        }
    }
}
