using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels
{
    public interface IOrderMakerVm : IPageVm
    {
        #region Commands

        ICommand CommandCheckout { get; }

        #endregion
    }
}