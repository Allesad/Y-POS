using System.Windows.Input;
using ReactiveUI;
using YumaPos.Client.Builders;
using YumaPos.Client.UI.ViewModels.Contracts;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.Pages
{
    public interface IOrderMakerVm : IPageVm
    {
        #region Properties
        
        string SearchItemText { get; set; }
        decimal Total { get; }

        IMenuCategoryItemVm[] Categories { get; }
        IMenuItemItemVm[] CategoryItems { get; }
        IReactiveDerivedList<IOrderedItem> OrderedItems { get; }

        IMenuCategoryItemVm SelectedCategory { get; set; }
        IOrderedItem SelectedItem { get; set; }

        #endregion

        #region Commands

        ICommand CommandAddCustomer { get; }
        ICommand CommandClear { get; }
        ICommand CommandVoid { get; }
        ICommand CommandPrint { get; }
        ICommand CommandCheckout { get; }

        #endregion
    }
}