using YumaPos.Client.UI.ViewModels.Contracts;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.Pages
{
    public interface INavMenuVm : IBaseVm
    {
        #region Properties

        string StoreName { get; }
        string TerminalName { get; }

        INavMenuItemVm[] Items { get; }
        INavMenuItemVm SelectedItem { get; set; }

        #endregion
    }
}