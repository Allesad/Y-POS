using System.Reactive.Linq;
using System.Windows;
using ReactiveUI;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.PageParts;

namespace Y_POS.Views.CheckoutParts
{
    /// <summary>
    /// Interaction logic for CheckoutPaymentCompleteView.xaml
    /// </summary>
    public partial class CheckoutPaymentCompleteView : BaseView
    {
        public CheckoutPaymentCompleteView()
        {
            InitializeComponent();
        }

        private PaymentCompleteVm ViewModel => (PaymentCompleteVm) DataContext;


        protected override void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            base.OnLoaded(sender, routedEventArgs);

            this.WhenAnyValue(view => view.ViewModel.ProgressState)
                .TakeUntil(closingObservable)
                .SubscribeToObserveOnUi(state =>
                {
                    OrderStartOrCompleteBtn.Content = state != OrderProgressState.NotStarted 
                        ? Core.Properties.Resources.Done
                        : Core.Properties.Resources.Start;
                    OrderStartOrCompleteBtn.Command = state != OrderProgressState.NotStarted ? ViewModel.CommandDone : ViewModel.CommandStart;
                });
        }
    }
}
