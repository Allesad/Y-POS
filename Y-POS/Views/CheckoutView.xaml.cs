using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Y_POS.Views.CheckoutParts;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for CheckoutView.xaml
    /// </summary>
    public partial class CheckoutView : BaseView
    {
        private bool _isPaid;

        public CheckoutView()
        {
            InitializeComponent();

            Content.Content = new CheckoutPaymentView();
            
            OperationsList.SetValue(ItemsControl.ItemsSourceProperty, new[]
            {
                new OperationItem((Geometry) FindResource("CustomerIcon"), Core.Properties.Resources.Customer, "Add Customer"),
                new OperationItem((Geometry) FindResource("PercentIcon"), Core.Properties.Resources.Discount, "10% - Happy Hour"),
                new OperationItem((Geometry) FindResource("DivideIcon"), Core.Properties.Resources.Split, "All on One"),
                new OperationItem((Geometry) FindResource("PromoIcon"), Core.Properties.Resources.Promo, "No")
            });
        }

        private void SwitchToPayment(object sender, RoutedEventArgs e)
        {
            OperationsList.UnselectAll();
            ((RadioButton) sender).IsChecked = true;
            Content.Content = new CheckoutPaymentView();
        }

        private void OperationsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserControl content = null;
            switch (OperationsList.SelectedIndex)
            {
                case -1:
                    content = new CheckoutPaymentView();
                    break;
                case 0:
                    content = new CheckoutCustomerView();
                    break;
                case 1:
                    content = new CheckoutDiscountView();
                    break;
                case 2:
                    content = new CheckoutSplittingsView();
                    break;
                case 3:
                    content = new CheckoutPromoView();
                    break;
            }
            if (content != null)
            {
                Content.Content = content;
                foreach (var rb in PaymentTypesContainer.Children.OfType<RadioButton>())
                {
                    rb.IsChecked = false;
                }
            }
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

        private void SwitchActionBar(object sender, RoutedEventArgs e)
        {
            if (!_isPaid) return;

            ActionBarLeftContainer.SetValue(Grid.ColumnSpanProperty, 1);
            Content.Content = new CheckoutPaymentView();
            RightActionButton.Title = Core.Properties.Resources.Void.ToUpper();
            PaymentTypesContainer.IsEnabled = true;
            OperationsList.IsEnabled = true;
        }

        private void Content_OnCheckout(object sender, RoutedEventArgs e)
        {
            ActionBarLeftContainer.SetValue(Grid.ColumnSpanProperty, 2);
            Content.Content = new CheckoutPaymentCompleteView();
            RightActionButton.Title = Core.Properties.Resources.Refund.ToUpper();
            PaymentTypesContainer.IsEnabled = false;
            OperationsList.IsEnabled = false;

            _isPaid = true;
        }
    }
}
