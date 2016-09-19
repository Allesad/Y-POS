using System;
using System.Windows.Input;
using ReactiveUI;
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

        #endregion

        #region Commands

        ICommand CommandCancel { get; }
        ICommand CommandDone { get; }

        #endregion

        #region Events

        event EventHandler CloseEvent;

        #endregion

        #region Methods

        void ProcessMenuItem(IMenuItemItemVm menuItem);
        void EditOrderItem(Guid orderId, IOrderedItemVm orderedItem);

        #endregion
    }
}