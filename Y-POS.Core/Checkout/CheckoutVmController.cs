using System;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Module.Checkout.Contracts;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Items.Impl;

namespace Y_POS.Core.Checkout
{
    public sealed class CheckoutVmController : ReactiveObject
    {
        #region Fields

        private readonly ICheckoutManager _checkoutManager;

        #endregion

        #region Properties

        private Guid OrderId => _checkoutManager.OrderId;

        [Reactive]
        public ReceiptItemVm[] Receipts { get; private set; }

        [Reactive]
        public ReceiptItemVm SelectedReceipt { get; set; }

        #endregion

        #region Constructor

        public CheckoutVmController(ICheckoutManager checkoutManager)
        {
            if (checkoutManager == null) throw new ArgumentNullException(nameof(checkoutManager));

            _checkoutManager = checkoutManager;

            this.WhenAnyValue(controller => controller._checkoutManager.Receipts).Skip(1)
                .Select(items => items.Select(item => new ReceiptItemVm(item)).ToArray())
                .SubscribeToObserveOnUi(vms => Receipts = vms);
        }

        #endregion

        #region Methods

        public void Checkout()
        {
            
        }

        #endregion
    }
}
