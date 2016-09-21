using System.Windows.Input;
using ReactiveUI;
using YumaPos.Client.UI.ViewModels.Contracts;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.Pages
{
    public enum OrderMakerDetailsType
    {
        Menu,
        ItemConstructor,
        GiftCards,
        AddCustomer
    }

    public interface IOrderMakerVm : IPageVm
    {
        #region Properties
        
        int OrderNumber { get; }
        string SearchItemText { get; set; }
        decimal Total { get; }
        
        string CustomerName { get; }
        IReactiveDerivedList<IOrderedItemVm> OrderedItems { get; }
        IOrderedItemVm SelectedItem { get; set; }

        OrderMakerDetailsType DetailsType { get; }
        ILifecycleVm DetailsVm { get; }

        #endregion

        #region Commands
        
        ICommand CommandAddCustomer { get; }
        ICommand CommandDeleteItem { get; }
        ICommand CommandClear { get; }
        ICommand CommandVoid { get; }
        ICommand CommandGiftCards { get; }
        ICommand CommandPrint { get; }
        ICommand CommandCheckout { get; }

        #endregion
    }
}