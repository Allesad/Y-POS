using System.Windows;
using System.Windows.Controls;
using ReactiveUI;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Pages;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for OrderMakerView.xaml
    /// </summary>
    public partial class OrderMakerView : BaseView
    {
        public OrderMakerView()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            base.OnLoaded(sender, routedEventArgs);

            ((IOrderMakerVm) DataContext).WhenAnyValue(vm => vm.DetailsType)
                .SubscribeToObserveOnUi(UpdateActionBar);
        }

        private void UpdateActionBar(OrderMakerDetailsType type)
        {
            switch (type)
            {
                case OrderMakerDetailsType.ItemConstructor:
                case OrderMakerDetailsType.GiftCards:
                case OrderMakerDetailsType.AddCustomer:
                    DetailsContainer.SetValue(Grid.RowSpanProperty, 2);
                    break;
                default:
                    DetailsContainer.SetValue(Grid.RowSpanProperty, 1);
                    break;
            }
        }
    }
}
