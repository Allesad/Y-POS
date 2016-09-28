using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;
using Y_POS.Core.ViewModels.Items.Impl;

namespace Y_POS.Core.ViewModels.Pages
{
    public interface ICheckoutVm : IPageVm
    {
        #region Properties

        ReceiptItemVm[] Receipts { get; }
        ReceiptItemVm SelectedReceipt { get; set; }
        IBaseVm OptionVm { get; }

        #endregion

        #region Commands

        ICommand CommandPrint { get; }
        ICommand CommandSendEmail { get; }
        ICommand CommandVoid { get; }
        ICommand CommandRefund { get; }
        ICommand CommandSwitchToPaymentType { get; }

        #endregion
    }
}