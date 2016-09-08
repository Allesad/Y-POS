using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels.Pages
{
    public interface IOrderMakerVm : IPageVm
    {
        #region Properties

        int OrderNumber { get; }

        #endregion

        #region Commands

        ICommand CommandCheckout { get; }

        #endregion
    }
}