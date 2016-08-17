using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace Y_POS.Views.CashDrawerParts
{
    /// <summary>
    /// Interaction logic for CashDrawerInitView.xaml
    /// </summary>
    public partial class CashDrawerInitView : UserControl
    {
        private ObservableCollection<object> _items; 

        public CashDrawerInitView()
        {
            InitializeComponent();

            _items = new ObservableCollection<object>(new[]
            {
                new {Value = 1, Qty = 5, IsCoins = false, Amount = 0m},
                new {Value = 5, Qty = 2, IsCoins = false, Amount = 0m},
                new {Value = 10, Qty = 4, IsCoins = false, Amount = 0m},
                new {Value = 20, Qty = 6, IsCoins = false, Amount = 0m},
                new {Value = 50, Qty = 1, IsCoins = false, Amount = 0m},
                new {Value = 100, Qty = 2, IsCoins = false, Amount = 0m},
                new {Value = 0, Qty = 0, IsCoins = true, Amount = 3.50m}
            }.ToList());

            BillsList.ItemsSource = _items;
        }
    }
}