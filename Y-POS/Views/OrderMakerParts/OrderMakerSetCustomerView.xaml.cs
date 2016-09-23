using YumaPos.Client.Helpers;
using YumaPos.Client.Services;
using Y_POS.Core.ViewModels;

namespace Y_POS.Views.OrderMakerParts
{
    /// <summary>
    /// Interaction logic for OrderMakerAddCustomerView.xaml
    /// </summary>
    public partial class OrderMakerSetCustomerView : BaseView
    {
        public OrderMakerSetCustomerView()
        {
            InitializeComponent();

            CustomerControl.DataContext = new SelectCustomerVm(ServiceLocator.Resolve<ICustomersService>());
        }
    }
}
