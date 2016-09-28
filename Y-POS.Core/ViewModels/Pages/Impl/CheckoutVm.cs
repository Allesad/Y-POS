using System;
using System.Collections.Generic;
using System.Windows.Input;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Common;
using YumaPos.Client.Module.Checkout.Contracts;
using YumaPos.Client.UI.ViewModels.Contracts;
using YumaPos.Client.UI.ViewModels.Impl;
using Y_POS.Core.ViewModels.Items.Impl;
using Y_POS.Core.ViewModels.PageParts;

namespace Y_POS.Core.ViewModels.Pages
{
    public sealed class CheckoutVm : PageVm, ICheckoutVm
    {
        #region Fields

        private readonly ICheckoutManager _checkoutManager;
        private readonly ISelectCustomerVm _selectCustomerVm;

        #endregion

        #region Properties

        public ReceiptItemVm[] Receipts { get; }
        public ReceiptItemVm SelectedReceipt { get; set; }

        [Reactive]
        public IBaseVm OptionVm { get; private set; }

        public ICommand CommandPrint { get; }
        public ICommand CommandSendEmail { get; }
        public ICommand CommandVoid { get; }
        public ICommand CommandRefund { get; }
        public ICommand CommandSwitchToPaymentType { get; }

        #endregion

        #region Constructor

        public CheckoutVm(ICheckoutManager checkoutManager, ISelectCustomerVm selectCustomerVm)
        {
            if (checkoutManager == null) throw new ArgumentNullException(nameof(checkoutManager));
            if (selectCustomerVm == null) throw new ArgumentNullException(nameof(selectCustomerVm));

            _selectCustomerVm = selectCustomerVm;
            _checkoutManager = checkoutManager;
        }

        #endregion

        #region Lifecycle

        protected override IEnumerable<ILifecycleVm> GetChildren()
        {
            return new ILifecycleVm[] { _selectCustomerVm };
        }

        protected override void InitCommands()
        {
            
        }

        protected override void InitLifetimeSubscriptions()
        {
            
        }

        protected override void OnCreate(IArgsBundle args)
        {
            Guid orderId = args?.GetGuid("id") ?? Guid.Empty;
            if (orderId == Guid.Empty)
            {
                throw new Exception("Order id is not set or has incorrect format!");
            }

            // Load receipts
            _checkoutManager.LoadOrder(orderId).Subscribe();

            OptionVm = _selectCustomerVm;
        }

        #endregion
    }
}
