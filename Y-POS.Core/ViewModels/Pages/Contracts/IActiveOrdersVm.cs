using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.Pages
{
    public interface IActiveOrdersVm : IPageVm, IEntityListVm<IActiveOrderItemVm, IActiveOrderItemVm[]>
    {
        #region Properties
        
        #endregion

        #region Commands

        ICommand CommandCreateOrder { get; }
        ICommand CommandCheckout { get; }
        ICommand CommandPrintOrder { get; }
        ICommand CommandVoid { get; }

        #endregion
    }
}