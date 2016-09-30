using System.Windows;
using System.Windows.Controls;
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
            
            Loaded += (sender, args) =>
            {
                ReceivedTb.Text = ((PaymentVm) DataContext).Received.ToString("c");
                TipsTb.Text = ((PaymentVm) DataContext).Tips.ToString("c");

                ReceivedTb.Focus();
            };
        }

        private void DoCheckout(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs args = new RoutedEventArgs(CheckoutEvent);
            RaiseEvent(args);
        }

        public static readonly RoutedEvent CheckoutEvent = EventManager.RegisterRoutedEvent("Checkout",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (CheckoutPaymentView));

        public event RoutedEventHandler Checkout
        {
            add { AddHandler(CheckoutEvent, value);}
            remove { RemoveHandler(CheckoutEvent, value);}
        }

        private void Input_OnGotFocus(object sender, RoutedEventArgs e)
        {
            KeypadControl.TargetBox = (TextBox) sender;
        }
    }
}
