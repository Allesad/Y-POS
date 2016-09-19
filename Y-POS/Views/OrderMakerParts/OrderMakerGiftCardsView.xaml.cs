using System.Windows.Controls;
using System.Windows.Media;
using YumaPos.Client.Common;
using YumaPos.Client.Helpers;
using Y_POS.Core.ViewModels.PageParts;

namespace Y_POS.Views.OrderMakerParts
{
    /// <summary>
    /// Interaction logic for OrderMakerGiftCardsView.xaml
    /// </summary>
    public partial class OrderMakerGiftCardsView : BaseView
    {
        public OrderMakerGiftCardsView()
        {
            InitializeComponent();

            var rs = ServiceLocator.Resolve<IResourcesService>();

            OperationsList.ItemsSource = new[]
            {
                new {Icon = (Geometry) FindResource("GiftCardIcon"), Title = rs.GetResource<string>("IssueCard")},
                new {Icon = (Geometry) FindResource("CircledPlusIcon"), Title = rs.GetResource<string>("RefillCard")},
                new {Icon = (Geometry) FindResource("DollarSignIcon"), Title = rs.GetResource<string>("CheckBalance")}
            };

            OperationsList.SelectedIndex = 0;
        }

        private void OperationsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext == null) return;

            switch (OperationsList.SelectedIndex)
            {
                case 0:
                    ((IGiftCardsVm) DataContext).CommandGoToIssueCard.Execute(null);
                    break;
                case 1:
                    ((IGiftCardsVm)DataContext).CommandGoToRefill.Execute(null);
                    break;
                case 2:
                    ((IGiftCardsVm)DataContext).CommandGoToBalance.Execute(null);
                    break;
            }
        }
    }
}
