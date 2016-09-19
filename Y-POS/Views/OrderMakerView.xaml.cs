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
                    DetailsContainer.SetValue(Grid.RowSpanProperty, 2);
                    /*MenuActions.Visibility = Visibility.Collapsed;
                    GiftCardsActions.Visibility = Visibility.Collapsed;
                    ItemConstructorActions.Visibility = Visibility.Visible;*/
                    break;
                case OrderMakerDetailsType.GiftCards:
                    DetailsContainer.SetValue(Grid.RowSpanProperty, 2);
                    /*MenuActions.Visibility = Visibility.Collapsed;
                    ItemConstructorActions.Visibility = Visibility.Collapsed;
                    GiftCardsActions.Visibility = Visibility.Visible;*/
                    break;
                default:
                    DetailsContainer.SetValue(Grid.RowSpanProperty, 1);
                    /*MenuActions.Visibility = Visibility.Visible;
                    GiftCardsActions.Visibility = Visibility.Collapsed;
                    ItemConstructorActions.Visibility = Visibility.Collapsed;*/
                    break;
            }
        }
    }
}
