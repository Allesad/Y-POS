using System.Windows;
using System.Windows.Controls;
using ReactiveUI;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.PageParts;

namespace Y_POS.Views.CheckoutParts
{
    /// <summary>
    /// Interaction logic for CheckoutPaymentView.xaml
    /// </summary>
    public partial class CheckoutPaymentView : UserControl
    {
        public CheckoutPaymentView()
        {
            InitializeComponent();

            ReceivedTb.Text = "0";
            TipsTb.Text = "0";
            
            Loaded += (sender, args) =>
            {
                ReceivedTb.Text = ((PaymentVm) DataContext).Received.ToString("N2");
                TipsTb.Text = ((PaymentVm) DataContext).Tips.ToString("N2");

                ReceivedTb.Focus();

                /*this.WhenAnyValue(view => view.ViewModel.Mode, view => view.ViewModel.IsMultiplePayment)
                    .SubscribeToObserveOnUi(_ =>
                    {
                        ReceivedTb.Text = "0";
                        TipsTb.Text = "0";
                        ReceivedTb.Focus();
                    });*/
            };
        }

        private PaymentVm ViewModel => (PaymentVm)DataContext;

        private void Input_OnGotFocus(object sender, RoutedEventArgs e)
        {
            KeypadControl.TargetBox = (TextBox) sender;
        }
    }
}
