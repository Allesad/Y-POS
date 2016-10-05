using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Y_POS.Core.Infrastructure;

namespace Y_POS.Core.Checkout
{
    public sealed class SelectedReceiptController : IDisposable
    {
        #region Fields

        private readonly BehaviorSubject<ReceiptItem> _sender; 

        #endregion

        #region Properties

        public IObservable<ReceiptItem> SelectedReceiptStream { get; }

        public ReceiptItem CurrentReceipt { get; private set; }

        #endregion

        #region Constructor

        public SelectedReceiptController()
        {
            _sender = new BehaviorSubject<ReceiptItem>(null);
            SelectedReceiptStream = _sender.AsObservable();
        }

        #endregion

        #region Methods

        public void SetSelectedReceipt(ReceiptItem receipt)
        {
            Guard.NotNull(receipt, nameof(receipt));

            CurrentReceipt = receipt;
            _sender.OnNext(receipt);
        }

        public void SetNoSelection()
        {
            CurrentReceipt = null;
            _sender.OnNext(null);
        }

        #endregion

        public void Dispose()
        {
            _sender.Dispose();
        }
    }
}
