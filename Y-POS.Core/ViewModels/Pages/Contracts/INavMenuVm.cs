using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels.Pages
{
    public interface INavMenuVm : IBaseVm
    {
        #region Properties

        string StoreName { get; }
        string TerminalName { get; }



        #endregion

        #region Commands

        ICommand CommandNavigate { get; }

        #endregion
    }
}