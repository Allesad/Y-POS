using System;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Module.Checkout.Contracts;
using YumaPos.Client.UI.ViewModels.Impl;

namespace Y_POS.Core.ViewModels.Items.Impl
{
    public sealed class ReceiptItemVm : BaseVm
    {
        #region Properties

        public int SplittingNumber { get; private set; }
        [Reactive]
        public string Total { get; private set; }
        [Reactive]
        public string Paid { get; private set; }
        [Reactive]
        public string Change { get; private set; }
        [Reactive]
        public bool IsPaid { get; private set; }
        [Reactive]
        public bool IsRefunded { get; private set; }
        [Reactive]
        public bool IsVoid { get; private set; }
        [Reactive]
        public bool IsTaxExempt { get; private set; }
        [Reactive]
        public string Receipt { get; private set; }

        #endregion

        #region Constructor

        public ReceiptItemVm(IReceiptItem model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            SplittingNumber = model.SplittingNumber;
            Total = model.Total;
            Paid = model.Paid;
            Change = model.Change;
            IsPaid = model.IsPaid;
            IsRefunded = model.IsRefunded;
            IsTaxExempt = model.IsTaxExempt;
            Receipt = model.ReceiptFormatted;
            IsVoid = model.IsVoid;
        }

        #endregion
    }
}
