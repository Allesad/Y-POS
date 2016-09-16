using System.Windows;
using Y_POS.Core.ViewModels.PageParts;

namespace Y_POS.Views.OrderMakerParts
{
    /// <summary>
    /// Interaction logic for OrderMakerMenuView.xaml
    /// </summary>
    public partial class OrderMakerMenuView : BaseView
    {
        public OrderMakerMenuView()
        {
            InitializeComponent();
        }
        
        private void MSC_OnCategoryItemSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ((IOrderMakerMenuVm) DataContext).CommandSelectMenuItem.Execute(e.NewValue);
        }
    }
}
