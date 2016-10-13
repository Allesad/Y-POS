using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;
using YumaPos.Shared.API.Enums;
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
        None,
        Payment,
        Customer,
        Discount,
        Splitting,
        Marketing,
        Refund,
        PaymentComplete
    }

    public interface ICheckoutVm : IPageVm
    {
        #region Properties

        ReceiptItemVm[] Receipts { get; }
        ReceiptItemVm SelectedReceipt { get; set; }
        IBaseVm OperationVm { get; }
        OperationType CurrentOperationType { get; }
        PaymentType CurrentPaymentType { get; }
        SplittingType CurrentSplittingType { get; }
        string CustomerName { get; }
        string DiscountName { get; }

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