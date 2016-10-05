using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Common;
using YumaPos.Client.UI.ViewModels.Contracts;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.API.Enums;
using Y_POS.Core.Checkout;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Items.Impl;
using Y_POS.Core.ViewModels.PageParts;

namespace Y_POS.Core.ViewModels.Pages
{
    public sealed class CheckoutVm : PageVm, ICheckoutVm
    {
        #region Fields

        private readonly CheckoutController _controller;
        private readonly ISelectCustomerVm _selectCustomerVm;

        private readonly SelectedReceiptController _selectedReceiptController;

        private PaymentVm _paymentVm;
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

        public SplittingType CurrentSplittingType { [ObservableAsProperty] get; }

        public string CustomerName { [ObservableAsProperty] get; }

        public string DiscountName { [ObservableAsProperty] get; }

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

        public CheckoutVm(CheckoutController controller,
            ISelectCustomerVm selectCustomerVm)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            if (selectCustomerVm == null) throw new ArgumentNullException(nameof(selectCustomerVm));

            _controller = controller;
            _selectCustomerVm = selectCustomerVm;

            _selectedReceiptController = new SelectedReceiptController();
        }

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
                .Subscribe(type => CurrentPaymentType = type));

            // Switch to operation type
            AddLifetimeSubscription(_commandSwitchToOperationType
                .Select(param => (OperationType) param)
                .Subscribe(type => CurrentOperationType = type));

            // Handle payment type switch
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm.CurrentPaymentType)
                .Subscribe(type =>
                {
                    CurrentOperationType = OperationType.Payment;
                    _paymentVm?.CommandSetPaymentType.Execute(type);
                }));

            // Handle operation type content switch
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm.CurrentOperationType)
                .Select(GetOperationVmForType)
                .ToPropertyEx(this, vm => vm.OperationVm));

            // Current customer
            AddLifetimeSubscription(_controller.CustomerNameStream
                .ToPropertyEx(this, vm => vm.CustomerName, string.Empty, SchedulerService.UiScheduler));

            // Current splitting type
            AddLifetimeSubscription(_controller.SplittingTypeStream
                .ToPropertyEx(this, vm => vm.CurrentSplittingType, SplittingType.AllOnOne, SchedulerService.UiScheduler));

            // Current discount
            AddLifetimeSubscription(_controller.DiscountStream
                .ToPropertyEx(this, vm => vm.DiscountName, "-", SchedulerService.UiScheduler));

            // Select customer
            /*AddLifetimeSubscription(Observable.FromEventPattern<CustomerSelectedEventArgs>(
                h => _selectCustomerVm.CustomerSelectedEvent += h,
                h => _selectCustomerVm.CustomerSelectedEvent -= h)
                .Select(pattern => pattern.EventArgs.Customer)
                .SelectMany(customer => _checkoutManager.SetCustomer(customer))
                .SubscribeToObserveOnUi(_ =>
                {
                    CurrentOperationType = OperationType.Payment;
                }));*/

            // Handle close events
            AddLifetimeSubscription(Observable.FromEventPattern(
                h => _selectCustomerVm.CancelEvent += h,
                h => _selectCustomerVm.CancelEvent -= h)
                .Subscribe(_ => CurrentOperationType = OperationType.Payment));

            // Receipts
            AddLifetimeSubscription(_controller.ReceiptsStream
                .Where(items => items != null)
                .Select(items => items.Select(item => new ReceiptItemVm(item)).ToArray())
                .SubscribeToObserveOnUi(receipts => Receipts = receipts));

            // Current receipt tracking
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm.SelectedReceipt).Skip(1)
                .Subscribe(receipt =>
                {
                    if (receipt != null)
                    {
                        _selectedReceiptController.SetSelectedReceipt(receipt.Model);
                    }
                    else
                    {
                        _selectedReceiptController.SetNoSelection();
                    }
                }));
        }

        protected override async void OnCreate(IArgsBundle args)
        {
            Guid orderId = args?.GetGuid("id") ?? Guid.Empty;
            if (orderId == Guid.Empty)
            {
                throw new Exception("Order id is not set or has incorrect format!");
            }

            // Load receipts
            try
            {
                await _controller.Init(orderId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
                DialogService.ShowErrorMessage($"Cannot load order: {ex.Message}");
                NavigationService.Back();
            }
        }

        #endregion

        #region Private methods

        private IBaseVm GetOperationVmForType(OperationType type)
        {
            switch (type)
            {
                case OperationType.Payment:
                    _paymentVm = _paymentVm ?? new PaymentVm(_controller, _selectedReceiptController);
                    return _paymentVm;
                case OperationType.PaymentComplete:
                    _paymentCompleteVm = _paymentCompleteVm ?? new PaymentCompleteVm(_controller);
                    return _paymentCompleteVm;
                case OperationType.Customer:
                    return _selectCustomerVm;
                case OperationType.Splitting:
                    _splittingsVm = _splittingsVm ?? CreateAndSetUpSplittingsVm();
                    return _splittingsVm;
                case OperationType.Discount:
                    _discountVm = _discountVm ?? CreateAndSetUpDiscountVm();
                    return _discountVm;
                case OperationType.Marketing:
                    _marketingVm = _marketingVm ?? CreateAndSetUpMarketingVm();
                    return _marketingVm;
                default:
                    return null;
            }
        }

        private SplittingsVm CreateAndSetUpSplittingsVm()
        {
            var vm = new SplittingsVm(_controller);

            AddLifetimeSubscription(Observable.FromEventPattern(
                    h => vm.CloseEvent += h,
                    h => vm.CloseEvent -= h)
                .Subscribe(_ => CurrentOperationType = OperationType.Payment));

            return vm;
        }

        private DiscountVm CreateAndSetUpDiscountVm()
        {
            var vm = new DiscountVm(_controller);

            AddLifetimeSubscription(Observable.FromEventPattern(
                            h => vm.CloseEvent += h,
                            h => vm.CloseEvent -= h)
                            .Subscribe(_ => CurrentOperationType = OperationType.Payment));
            return vm;
        }

        private MarketingVm CreateAndSetUpMarketingVm()
        {
            var vm = new MarketingVm(_controller);

            AddLifetimeSubscription(Observable.FromEventPattern(
                            h => vm.CloseEvent += h,
                            h => vm.CloseEvent -= h)
                            .Subscribe(_ => CurrentOperationType = OperationType.Payment));

            return vm;
        }

        #endregion
    }
}