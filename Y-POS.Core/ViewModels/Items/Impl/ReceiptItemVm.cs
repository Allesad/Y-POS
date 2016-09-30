using System;
using YumaPos.Client.Module.Checkout.Contracts;
using YumaPos.Client.UI.ViewModels.Impl;

namespace Y_POS.Core.ViewModels.Items.Impl
{
    public sealed class ReceiptItemVm : BaseVm
    {
        #region Properties

        public int SplittingNumber => Model.SplittingNumber;
        public string Total => Model.Total;
        public string Paid => Model.Paid;
        public string Change => Model.Change;
        public bool IsPaid => Model.IsPaid;
        public bool IsRefunded => Model.IsRefunded;
        public bool IsVoid => Model.IsVoid;
        public bool IsTaxExempt => Model.IsTaxExempt;
        public string Receipt => Model.ReceiptFormatted;

        public IReceiptItem Model { get; }

        #endregion

        #region Constructor

        public ReceiptItemVm(IReceiptItem model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            
            Model = model;
        }

        #endregion

        public override string ToString()
        {
            return $"Receipt {SplittingNumber}\tTotal: {Total}";
        }
    }
}
