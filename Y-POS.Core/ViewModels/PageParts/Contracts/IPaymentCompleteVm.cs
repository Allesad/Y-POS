using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels.PageParts
{
    public interface IPaymentCompleteVm : IBaseVm
    {
        #region Properties

        #endregion

        #region Commands

        ICommand CommandCloseOrder { get; }
        ICommand CommandCloseAndCreateNewOrder { get; }
        ICommand CommandNewOrder { get; }
        ICommand CommandActiveOrders { get; }

        #endregion
    }
}