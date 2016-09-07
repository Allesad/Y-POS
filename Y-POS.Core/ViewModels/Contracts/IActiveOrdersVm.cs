using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels
{
    public interface IActiveOrdersVm : IPageVm
    {
        #region Properties

        #endregion

        #region Commands

        ICommand CommandCreateOrder { get; }

        #endregion
    }
}