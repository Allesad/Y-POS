using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Common;
using YumaPos.Client.Module.Checkout.Contracts;
using YumaPos.Client.UI.ViewModels.Contracts;
using YumaPos.Client.UI.ViewModels.Impl;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Items.Impl;
using Y_POS.Core.ViewModels.PageParts;

namespace Y_POS.Core.ViewModels.Pages
{
    public sealed class CheckoutVm : PageVm, ICheckoutVm
    {
        #region Fields

        private readonly ICheckoutManager _checkoutManager;
        private readonly IPaymentVm _paymentVm;
        private readonly ISelectCustomerVm _selectCustomerVm;
        private readonly IDiscountVm _discountVm;
        private readonly ISplittingsVm _splittingsVm;
        private readonly IMarketingVm _marketingVm;

        private ReactiveCommand<object> _commandSwitchToPaymentType;
        private ReactiveCommand<object> _commandSwitchToOperationType;

        private PaymentType _currentPaymentType = PaymentType.Cash;

        #endregion

        #region Properties

        [Reactive]
        public ReceiptItemVm[] Receipts { get; private set; }

        [Reactive]
        public ReceiptItemVm SelectedReceipt { get; set; }

        public extern ILifecycleVm OperationVm { [ObservableAsProperty] get; }

        [Reactive]
        public OperationType CurrentOperationType { get; private set; }

        #endregion

        #region Commands

        public ICommand CommandPrint { get; }
        public ICommand CommandSendEmail { get; }
        public ICommand CommandVoid { get; }
        public ICommand CommandRefund { get; }
        public ICommand CommandSwitchToPaymentType => _commandSwitchToPaymentType;
        public ICommand CommandSwitchToOperationType => _commandSwitchToOperationType;

        #endregion

        #region Constructor

        public CheckoutVm(ICheckoutManager checkoutManager, ISelectCustomerVm selectCustomerVm)
        {
            _checkoutManager = checkoutManager;
            _selectCustomerVm = selectCustomerVm;
        }

        /*public CheckoutVm(ICheckoutManager checkoutManager, 
            ISelectCustomerVm selectCustomerVm, 
            IPaymentVm paymentVm, 
            IDiscountVm discountVm, 
            ISplittingsVm splittingsVm, 
            IMarketingVm marketingVm)
        {
            if (checkoutManager == null) throw new ArgumentNullException(nameof(checkoutManager));
            if (selectCustomerVm == null) throw new ArgumentNullException(nameof(selectCustomerVm));

            _selectCustomerVm = selectCustomerVm;
            _paymentVm = paymentVm;
            _discountVm = discountVm;
            _splittingsVm = splittingsVm;
            _marketingVm = marketingVm;
            _checkoutManager = checkoutManager;
        }*/

        #endregion

        #region Lifecycle

        protected override IEnumerable<ILifecycleVm> GetChildren()
        {
            return new ILifecycleVm[] {_selectCustomerVm};
        }

        protected override void InitCommands()
        {
            _commandSwitchToPaymentType = ReactiveCommand.Create();
            _commandSwitchToOperationType = ReactiveCommand.Create();
        }

        protected override void InitLifetimeSubscriptions()
        {
            // Switch to payment type
            AddLifetimeSubscription(_commandSwitchToPaymentType
                .Select(param => (PaymentType) param)
                .Subscribe(type => _currentPaymentType = type));

            // Switch to operation type
            AddLifetimeSubscription(_commandSwitchToOperationType
                .Select(param => (OperationType) param)
                .Subscribe(type => CurrentOperationType = type));

            // Handle operation type content switch
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm.CurrentOperationType)
                .Select(GetOperationVmForType)
                .ToPropertyEx(this, vm => vm.OperationVm));

            // Select customer
            AddLifetimeSubscription(Observable.FromEventPattern<CustomerSelectedEventArgs>(
                h => _selectCustomerVm.CustomerSelectedEvent += h,
                h => _selectCustomerVm.CustomerSelectedEvent -= h)
                .Select(pattern => pattern.EventArgs.Customer)
                .Subscribe(_ => CurrentOperationType = OperationType.Payment));

            // Handle close events
            AddLifetimeSubscription(Observable.FromEventPattern(
                h => _selectCustomerVm.CancelEvent += h,
                h => _selectCustomerVm.CancelEvent -= h)
                .Subscribe(_ => CurrentOperationType = OperationType.Payment));
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
        }

        #endregion

        #region Private methods

        private ILifecycleVm GetOperationVmForType(OperationType type)
        {
            switch (type)
            {
                case OperationType.Customer:
                    return _selectCustomerVm;
                default:
                    return null;
            }
        }

        #endregion
    }
}