using System.Windows.Input;
using ReactiveUI;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels
{
    public interface IPinVm : IPageVm, IRoutableViewModel
    {
        #region Properties

        #endregion

        #region Commands

        ICommand CommandLogin { get; }

        #endregion
    }
}