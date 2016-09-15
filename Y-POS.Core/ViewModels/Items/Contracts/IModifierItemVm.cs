using System.Windows.Input;
using YumaPos.Client.Common;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels.Items.Contracts
{
    public interface IModifierItemVm : IBaseVm, IIdentifiable, IImageContainable
    {
        #region Properties

        string Title { get; }
        decimal Price { get; }
        int Qty { get; }
        int GroupMaxQty { get; }
        int MaxQty { get; }
        bool CanModifyQty { get; }
        bool IsSkipOption { get; }

        #endregion

        #region Commands

        ICommand CommandSelectModifier { get; }
        ICommand CommandIncreaseQty { get; }
        ICommand CommandDecreaseQty { get; }

        #endregion
    }
}