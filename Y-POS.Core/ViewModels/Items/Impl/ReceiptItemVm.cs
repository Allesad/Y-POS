using System;
using YumaPos.Client.UI.ViewModels.Impl;
using Y_POS.Core.Checkout;

namespace Y_POS.Core.ViewModels.Items.Impl
{
    public sealed class ReceiptItemVm : BaseVm
    {
        #region Properties

        public int SplittingNumber => Model.SplittingNumber;
        public decimal Total => Model.Total;
        public decimal Paid => Model.TotalPaid;
        public decimal Change => Model.Change;
        public bool IsPaid => Model.IsPaid;
        public bool IsRefunded => Model.IsRefunded;
        public bool IsVoid => Model.IsVoid;
        public bool IsTaxExempt => Model.IsTaxExempt;
        //public string Receipt => Model.ReceiptFormatted;

        public ReceiptItem Model { get; }

        #endregion

        #region Constructor

        public ReceiptItemVm(ReceiptItem model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            
            Model = model;
        }

        #endregion

        public override string ToString()
        {
            return $"Receipt {SplittingNumber.ToString("D")}\tTotal: {Total.ToString("C")}";
        }
    }
}
