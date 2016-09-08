using YumaPos.Client.Navigation;
using Y_POS.Core.ViewModels;
using Y_POS.Core.ViewModels.Pages;

namespace Y_POS.Core
{
    public static class AppNavigation
    {
        #region Pages URI

        public static readonly NavUri Login = new NavUri(string.Empty, typeof(ILoginVm).Name);
        public static readonly NavUri PinLogin = new NavUri(string.Empty, typeof(IPinVm).Name);
        public static readonly NavUri ActiveOrders = new NavUri(string.Empty, typeof(IActiveOrdersVm).Name);
        public static readonly NavUri ClosedOrders = new NavUri(string.Empty, typeof(IClosedOrdersVm).Name);
        public static readonly NavUri Cashdrawer = new NavUri(string.Empty, typeof(ICashdrawerVm).Name);
        public static readonly NavUri Reports = new NavUri(string.Empty, typeof(IReportsVm).Name);
        public static readonly NavUri Settings = new NavUri(string.Empty, typeof(ISettingsVm).Name);
        public static readonly NavUri OrderMaker = new NavUri(string.Empty, typeof(IOrderMakerVm).Name);
        public static readonly NavUri Checkout = new NavUri(string.Empty, typeof(ICheckoutVm).Name);
        
        #endregion
    }
}
