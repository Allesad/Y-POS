using System;
using ReactiveUI;
using YumaPos.Client.UI.ViewModels.Contracts;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.PageParts
{
    public interface IOrderItemConstructorVm : ILifecycleVm
    {
        #region Properties

        IModifierItemVm[] RelatedModifiers { get; }
        IReadOnlyReactiveList<IModifiersGroupItemVm> SelectedGroups { get; }
        IModifiersGroupItemVm SelectedGroup { get; }
        string MenuItemTitle { get; }
        decimal MenuItemPrice { get; }

        string GroupTitle { get; }
        int MaxGroupQty { get; }
        string RequiredStatus { get; }
        decimal Total { get; }

        #endregion

        #region Methods

        void ProcessMenuItem(IMenuItemItemVm menuItem);
        void EditOrderItem(Guid orderId, IOrderedItemVm orderedItem);
        void Cancel();

        #endregion
    }
}