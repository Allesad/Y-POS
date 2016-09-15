using YumaPos.Client.Common;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels.Items.Contracts
{
    public interface IOrderedItemVm : IBaseVm, IIdentifiable
    {
        #region Properties

        string Title { get; }
        string Description { get; }
        decimal Price { get; }
        int Qty { get; }

        #endregion
    }
}