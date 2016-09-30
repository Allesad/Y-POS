using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Common;
using YumaPos.Client.Module.Checkout.Contracts;
using YumaPos.Client.UI.ViewModels.Contracts;
using YumaPos.Client.UI.ViewModels.Impl;
using Y_POS.Core.Checkout;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Items.Impl;
using Y_POS.Core.ViewModels.PageParts;

namespace Y_POS.Core.ViewModels.Pages
{
    public sealed class CheckoutVm : PageVm, ICheckoutVm
    {
        #region Fields

        private readonly ICheckoutManager _checkoutManager;
        private readonly CheckoutVmController _controller;
        private readonly IPaymentVm _paymentVm;
        private readonly ISelectCustomerVm _selectCustomerVm;

        private DiscountVm _discountVm;
        private SplittingsVm _splittingsVm;
        private MarketingVm _marketingVm;
        private PaymentCompleteVm _paymentCompleteVm;

        private ReactiveCommand<object> _commandSwitchToPaymentType;
        private ReactiveCommand<object> _commandSwitchToOperationType;
        
        #endregion

        #region Properties
        
        [Reactive]
        public ReceiptItemVm[] Receipts { get; private set; }

        [Reactive]
        public ReceiptItemVm SelectedReceipt { get; set; }

        public extern IBaseVm OperationVm { [ObservableAsProperty] get; }

        [Reactive]
        public OperationType CurrentOperationType { get; private set; }

        [Reactive]
        public PaymentType CurrentPaymentType { get; private set; }

        [Reactive]
        public string CustomerName { get; private set; }

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

        public CheckoutVm(ICheckoutManager checkoutManager, 
            CheckoutVmController controller,
            ISelectCustomerVm selectCustomerVm, 
            IPaymentVm paymentVm)
        {
            if (checkoutManager == null) throw new ArgumentNullException(nameof(checkoutManager));
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            if (selectCustomerVm == null) throw new ArgumentNullException(nameof(selectCustomerVm));
            if (paymentVm == null) throw new ArgumentNullException(nameof(paymentVm));

            _checkoutManager = checkoutManager;
            _controller = controller;
            _selectCustomerVm = selectCustomerVm;
            _paymentVm = paymentVm;
        }

        #endregion

        #region Lifecycle

        protected override IEnumerable<ILifecycleVm> GetChildren()
        {
            return new ILifecycleVm[] { _selectCustomerVm, _paymentVm };
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
                .Subscribe(type => CurrentPaymentType = type));

            // Switch to operation type
            AddLifetimeSubscription(_commandSwitchToOperationType
                .Select(param => (OperationType) param)
                .Subscribe(type => CurrentOperationType = type));

            // Handle payment type switch
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm.CurrentPaymentType)
                .Subscribe(type => CurrentOperationType = OperationType.Payment));

            // Handle operation type content switch
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm.CurrentOperationType)
                .Select(GetOperationVmForType)
                .ToPropertyEx(this, vm => vm.OperationVm));

            // Current customer
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm._checkoutManager.CustomerName)
                .SubscribeToObserveOnUi(customerName => CustomerName = customerName));

            // Select customer
            AddLifetimeSubscription(Observable.FromEventPattern<CustomerSelectedEventArgs>(
                h => _selectCustomerVm.CustomerSelectedEvent += h,
                h => _selectCustomerVm.CustomerSelectedEvent -= h)
                .Select(pattern => pattern.EventArgs.Customer)
                .SelectMany(customer => _checkoutManager.SetCustomer(customer))
                .SubscribeToObserveOnUi(_ =>
                {
                    CurrentOperationType = OperationType.Payment;
                }));

            // Handle close events
            AddLifetimeSubscription(Observable.FromEventPattern(
                    h => _selectCustomerVm.CancelEvent += h,
                    h => _selectCustomerVm.CancelEvent -= h)
                .Subscribe(_ => CurrentOperationType = OperationType.Payment));

            // Receipts
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm._checkoutManager.Receipts).Skip(1)
                .Select(items => items.Select(item => new ReceiptItemVm(item)).ToArray())
                .Subscribe(vms => Receipts = vms));

            // Current receipt tracking
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm._checkoutManager.CurrentReceipt).Skip(1)
                .SubscribeToObserveOnUi(item => SelectedReceipt = Receipts.First(vm => vm.Model == item)));
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm.SelectedReceipt).Skip(1)
                .Where(vm => vm != null)
                .Select(vm => vm.Model)
                .Subscribe(receiptModel => _checkoutManager.CurrentReceipt = receiptModel));
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

        private IBaseVm GetOperationVmForType(OperationType type)
        {
            switch (type)
            {
                case OperationType.Payment:
                    return _paymentVm;
                case OperationType.PaymentComplete:
                    _paymentCompleteVm = _paymentCompleteVm ?? new PaymentCompleteVm(_controller);
                    return _paymentCompleteVm;
                case OperationType.Customer:
                    return _selectCustomerVm;
                case OperationType.Splitting:
                    if (_splittingsVm == null)
                    {
                        _splittingsVm = new SplittingsVm(_controller, _checkoutManager);
                        AddLifetimeSubscription(Observable.FromEventPattern(
                                h => _splittingsVm.CloseEvent += h,
                                h => _splittingsVm.CloseEvent -= h)
                            .Subscribe(_ => CurrentOperationType = OperationType.Payment));
                    }
                    return _splittingsVm;
                case OperationType.Discount:
                    if (_discountVm == null)
                    {
                        _discountVm = new DiscountVm(_controller);
                        AddLifetimeSubscription(Observable.FromEventPattern(
                                h => _discountVm.CloseEvent += h,
                                h => _discountVm.CloseEvent -= h)
                            .Subscribe(_ => CurrentOperationType = OperationType.Payment));
                    }
                    return _discountVm;
                case OperationType.Marketing:
                    if (_marketingVm == null)
                    {
                        _marketingVm = new MarketingVm(_controller);
                        AddLifetimeSubscription(Observable.FromEventPattern(
                                h => _marketingVm.CloseEvent += h,
                                h => _marketingVm.CloseEvent -= h)
                            .Subscribe(_ => CurrentOperationType = OperationType.Payment));
                    }
                    return _marketingVm;
                default:
                    return null;
            }
        }

        #endregion
    }
}