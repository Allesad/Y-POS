using System.Windows.Input;
using YumaPos.Client.Navigation;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels.Items.Contracts
{
    public interface INavMenuItemVm : IBaseVm
    {
        #region Properties

        NavUri TargetUri { get; }
        string Title { get; }

        #endregion

        #region Commands

        ICommand CommandNavigate { get; }

        #endregion
    }
}