using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ReactiveUI;
using YumaPos.Client.Hardware;
using YumaPos.Client.Helpers;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Pages;
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
        }

        private CheckoutVm ViewModel => (CheckoutVm) DataContext;

        protected override void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            base.OnLoaded(sender, routedEventArgs);

            this.WhenAnyValue(view => view.ViewModel.Receipts)
                .Where(vms => vms != null)
                .Select(vms => vms.Any(vm => vm.IsPaid))
                .SubscribeToObserveOnUi(hasPaidReceipts =>
                {
                    RightActionButton.Command = hasPaidReceipts
                        ? ViewModel.CommandRefund
                        : ViewModel.CommandVoid;
                    RightActionButton.Title = hasPaidReceipts
                        ? Core.Properties.Resources.Refund.ToUpper()
                        : Core.Properties.Resources.Void.ToUpper();
                });
        }

        /*private void SwitchToPayment(object sender, RoutedEventArgs e)
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
            else
            {
                Content.Content = ((ICheckoutVm) DataContext).OperationVm;
            }
        }*/

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

        private void Content_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            int i = 0;
        }

        private void CommandPrint_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ServiceLocator.Resolve<IPrintService>().PrintReceipt(null);
        }
    }
}
