using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for CheckoutView.xaml
    /// </summary>
    public partial class CheckoutView : UserControl
    {
        public CheckoutView()
        {
            InitializeComponent();

            OperationsList.SetValue(ItemsControl.ItemsSourceProperty, new[]
            {
                new OperationItem((Geometry) FindResource("CustomerIcon"), "Customer", "Add Customer"),
                new OperationItem((Geometry) FindResource("PercentIcon"), "Discount", "10% - Happy Hour"),
                new OperationItem((Geometry) FindResource("DivideIcon"), "Split", "All on One"),
                new OperationItem((Geometry) FindResource("PromoIcon"), "Promo", "No")
            });
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            ((Window)Window.GetWindow(this)).Content = new OrderMakerView();
        }

        private class OperationItem
        {
            public Geometry Icon { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }

            public OperationItem(Geometry icon, string title, string content)
            {
                Icon = icon;
                Title = title;
                Content = content;
            }
        }
    }
}
