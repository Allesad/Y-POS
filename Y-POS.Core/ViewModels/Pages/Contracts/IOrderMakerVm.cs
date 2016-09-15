using System.Windows.Input;
using ReactiveUI;
using YumaPos.Client.UI.ViewModels.Contracts;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.Pages
{
    public interface IOrderMakerVm : IPageVm
    {
        #region Properties
        
        string SearchItemText { get; set; }
        decimal Total { get; }
        
        IReactiveDerivedList<IOrderedItemVm> OrderedItems { get; }
        IOrderedItemVm SelectedItem { get; set; }

        ILifecycleVm DetailsVm { get; }

        #endregion

        #region Commands
        
        ICommand CommandAddCustomer { get; }
        ICommand CommandDeleteItem { get; }
        ICommand CommandClear { get; }
        ICommand CommandVoid { get; }
        ICommand CommandPrint { get; }
        ICommand CommandCheckout { get; }

        // Item constructor commands
        ICommand CommandCancelItemConstructor { get; }
        ICommand CommandSubmitItemConstructor { get; }

        #endregion
    }
}