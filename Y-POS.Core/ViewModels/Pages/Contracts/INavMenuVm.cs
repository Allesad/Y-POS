using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels.Pages
{
    public interface INavMenuVm : IBaseVm
    {
        #region Properties

        #endregion

        #region Commands

        ICommand CommandNavigate { get; }

        #endregion
    }
}