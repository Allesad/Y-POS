using System;
using System.Collections.Generic;
using ReactiveUI;
using YumaPos.Client.Builders;
using YumaPos.Client.UI.ViewModels.Contracts;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.PageParts
{
    public interface IOrderItemConstructorVm : ILifecycleVm
    {
        #region Properties

        Guid? MenuItemId { get; }

        IModifierItemVm[] RelatedModifiers { get; }
        IReadOnlyReactiveList<IModifiersGroupItemVm> SelectedGroups { get; }
        IModifiersGroupItemVm SelectedGroup { get; }
        string MenuItemTitle { get; }
        decimal MenuItemPrice { get; }

        string GroupTitle { get; }
        int MaxGroupQty { get; }
        string RequiredStatus { get; }
        decimal Total { get; }

        bool CanCompleteItem { get; } 

        #endregion

        #region Methods

        void ProcessMenuItem(IMenuItemItemVm menuItem);
        void EditOrderItem(Guid orderId, IOrderedItemVm orderedItem);
        void Cancel();

        IEnumerable<ModifierToAdd> GetRelatedModifiers();
        IEnumerable<ModifierToAdd> GetCommonModifiers();

        #endregion
    }
}