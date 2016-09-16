using ReactiveUI;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels.Items.Contracts
{
    public interface IModifiersGroupItemVm : IBaseVm
    {
        #region Properties

        string Title { get; }
        bool IsTitleVisible { get; }
        bool IsRequired { get; }
        int MaxQty { get; }
        IReadOnlyReactiveList<IModifierItemVm> Modifiers { get; } 

        #endregion
    }
}