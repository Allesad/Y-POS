using YumaPos.Client.Common;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels.Items.Contracts
{
    public interface IGiftCardItemVm : IBaseVm, IIdentifiable
    {
        #region Properties

        string Title { get; }
        decimal Price { get; }

        #endregion
    }
}