using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels.Pages
{
    public interface IAppMainVm : IMainVm<IPageVm>
    {
        #region Properties

        INavMenuVm NavMenuVm { get; }

        #endregion
    }
}