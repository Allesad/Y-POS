using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;
using Y_POS.Core.ViewModels.Items.Impl;

namespace Y_POS.Core.ViewModels.Pages
{
    public enum PaymentType
    {
        Cash,
        Card,
        Mobile,
        GiftCard,
        Points,
        Multiple
    }

    public enum OperationType
    {
        Payment,
        Customer,
        Discount,
        Splitting,
        Marketing
    }

    public interface ICheckoutVm : IPageVm
    {
        #region Properties

        ReceiptItemVm[] Receipts { get; }
        ReceiptItemVm SelectedReceipt { get; set; }
        ILifecycleVm OperationVm { get; }

        #endregion

        #region Commands

        ICommand CommandPrint { get; }
        ICommand CommandSendEmail { get; }
        ICommand CommandVoid { get; }
        ICommand CommandRefund { get; }
        ICommand CommandSwitchToPaymentType { get; }
        ICommand CommandSwitchToOperationType { get; }

        #endregion
    }
}