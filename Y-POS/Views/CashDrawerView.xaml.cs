using System.Windows.Controls;
using System.Windows.Media;

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
            
            OperationsList.SetValue(ItemsControl.ItemsSourceProperty, new[]
            {
                new OperationItem((Geometry) FindResource("PerformanceIcon"), "PERFORMANCE"),
                new OperationItem((Geometry) FindResource("CashierInIcon"), "CASHIER IN"),
                new OperationItem((Geometry) FindResource("CashdrawerCheckIcon"), "CASHIER CHECK"),
                new OperationItem((Geometry) FindResource("BankIcon"), "BANK WITHDRAW"),
                new OperationItem((Geometry) FindResource("BackIcon"), "CASH IN"),
                new OperationItem((Geometry) FindResource("BackIcon"), "CASH OUT")
            });
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
    }
}
