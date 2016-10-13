using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ReactiveUI;
using YumaPos.Client.Hardware;
using YumaPos.Client.Helpers;
using Y_POS.Core.Extensions;
using Y_POS.Core.Infrastructure;
using Y_POS.Core.ViewModels.Pages;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for CheckoutView.xaml
    /// </summary>
    public partial class CheckoutView : BaseView
    {
        public CheckoutView()
        {
            InitializeComponent();
        }

        private CheckoutVm ViewModel => (CheckoutVm) DataContext;

        protected override void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            base.OnLoaded(sender, routedEventArgs);

            this.WhenAnyValue(view => view.ViewModel.Receipts)
                .TakeUntil(closingObservable)
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

            this.WhenAnyValue(view => view.ViewModel.SelectedReceipt)
                .TakeUntil(closingObservable)
                .Select(vm => vm?.ReceiptHtml)
                .SubscribeToObserveOnUi(receiptHtml =>
                {
                    if (receiptHtml.IsEmpty())
                    {
                        ReceiptControl.GoToHome();
                        return;
                    }
                    ReceiptControl.LoadHTML(receiptHtml);
                });

            this.WhenAnyValue(view => view.ViewModel.CurrentOperationType)
                .Select(type => type == CheckoutOperationType.PaymentComplete)
                .SubscribeToObserveOnUi(isComplete => ActionBarLeftContainer.SetValue(Grid.ColumnSpanProperty, isComplete ? 2 : 1));
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            base.OnUnloaded(sender, routedEventArgs);

            if (!ReceiptControl.IsDisposed) ReceiptControl.Dispose();
        }

        private void CommandPrint_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ServiceLocator.Resolve<IPrintService>().PrintReceipt(null);
        }
    }
}
