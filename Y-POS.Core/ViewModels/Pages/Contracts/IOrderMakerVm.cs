using System.Windows.Input;
using ReactiveUI;
using YumaPos.Client.Builders;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels.Pages
{
    public interface IOrderMakerVm : IPageVm
    {
        #region Properties

        int OrderNumber { get; }
        string SearchItemText { get; set; }
        decimal Total { get; }

        IReactiveDerivedList<IOrderedItem> OrderedItems { get; }
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