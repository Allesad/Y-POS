using System;
using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.PageParts
{
    public interface IOrderMakerMenuVm : ILifecycleVm
    {
        #region Properties

        string SearchText { get; set; }

        IMenuCategoryItemVm[] Categories { get; }
        IMenuItemItemVm[] CategoryItems { get; }

        IMenuCategoryItemVm SelectedCategory { get; set; }

        #endregion

        #region Commands

        ICommand CommandSelectMenuItem { get; }

        #endregion

        #region Events

        event EventHandler<MenuItemSelectedEventArgs> MenuItemSelected;

        #endregion
    }

    public class MenuItemSelectedEventArgs : EventArgs
    {
        public IMenuItemItemVm MenuItem { get; private set; }
        public bool HasModifiers { get; private set; }

        public MenuItemSelectedEventArgs(IMenuItemItemVm menuItem, bool hasModifiers)
        {
            MenuItem = menuItem;
            HasModifiers = hasModifiers;
        }
    }
}