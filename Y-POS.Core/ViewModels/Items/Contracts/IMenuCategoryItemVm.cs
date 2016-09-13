using YumaPos.Client.Common;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels.Items.Contracts
{
    public interface IMenuCategoryItemVm : IBaseVm, IIdentifiable, IImageContainable
    {
        string Title { get; }
    }
}