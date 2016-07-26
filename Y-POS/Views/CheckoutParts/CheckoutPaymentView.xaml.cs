using System.Windows;
using System.Windows.Controls;

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
    }
}
