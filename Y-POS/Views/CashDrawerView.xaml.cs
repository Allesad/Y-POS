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
                new OperationItem((Geometry) FindResource("CashierInIcon"), "CASHIER IN"),
                new OperationItem((Geometry) FindResource("CashdrawerCheckIcon"), "CASHDRAWER CHECK"),
                new OperationItem((Geometry) FindResource("BankIcon"), "BANK WITHDRAW"),
                new OperationItem((Geometry) FindResource("ArrowDownIcon"), "CASH IN"),
                new OperationItem((Geometry) FindResource("ArrowUpIcon"), "CASH OUT"),
                new OperationItem((Geometry) FindResource("PerformanceIcon"), "PERFORMANCE")
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
                case 1:
                    ContentContainer.Content = new CashDrawerInitView();
                    break;
                case 2:
                    ContentContainer.Content = new CashDrawerAmountUpdateView
                    {
                        Title = "Bank Withdraw:"
                    };
                    break;
                case 3:
                    ContentContainer.Content = new CashDrawerAmountUpdateView
                    {
                        Title = "Cash In:"
                    };
                    break;
                case 4:
                    ContentContainer.Content = new CashDrawerAmountUpdateView
                    {
                        Title = "Cash Out:"
                    };
                    break;
                case 5:
                    ContentContainer.Content = new CashDrawerPerformanceView();
                    break;
            }
            UpdateActionBar(list.SelectedIndex);
        }

        private void UpdateActionBar(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 5:
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
