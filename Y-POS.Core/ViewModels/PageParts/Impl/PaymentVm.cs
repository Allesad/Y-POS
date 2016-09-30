using System;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Module.Checkout.Contracts;
using YumaPos.Client.UI.ViewModels.Impl;
using Y_POS.Core.Checkout;
using Y_POS.Core.Extensions;

namespace Y_POS.Core.ViewModels.PageParts
{
    public class PaymentVm : LifecycleVm, IPaymentVm
    {
        private readonly CheckoutVmController _controller;
        private readonly ICheckoutManager _checkoutManager;

        #region Fields

        #endregion

        #region Properties

        [Reactive]
        public decimal Received { get; set; }

        [Reactive]
        public decimal Tips { get; set; }

        [Reactive]
        public decimal Subtotal { get; private set; }

        [Reactive]
        public decimal Total { get; private set; }
        
        public extern decimal Change { [ObservableAsProperty] get; }

        #endregion

        #region Commands

        public ICommand CommandCheckout { get; }

        #endregion

        #region Constructor

        public PaymentVm(CheckoutVmController controller, ICheckoutManager checkoutManager)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            if (checkoutManager == null) throw new ArgumentNullException(nameof(checkoutManager));

            _controller = controller;
            _checkoutManager = checkoutManager;

            var canPayStream = this.WhenAny(vm => vm.Total, vm => vm.Received, vm => vm.Tips,
                (total, received, tips) => (received.Value - total.Value - tips.Value) >= 0);

            var cmdCheckout = ReactiveCommand.Create(canPayStream);
            cmdCheckout.Subscribe(_ => DialogService.ShowNotificationMessage($"Received: {Received.ToString("c")},\n" +
                                                                             $"Tips: {Tips.ToString("c")}"));

            CommandCheckout = cmdCheckout;

            this.WhenAnyValue(vm => vm._checkoutManager.CurrentReceipt).Skip(1)
                .Where(item => item != null)
                .SubscribeToObserveOnUi(receipt =>
                {
                    Subtotal = receipt.SubTotalValue;
                    Total = receipt.TotalValue;
                });

            this.WhenAny(vm => vm.Total, vm => vm.Received, vm => vm.Tips,
                (total, received, tips) => received.Value - total.Value - tips.Value)
                .Select(leftover => Math.Max(leftover, 0))
                .ToPropertyEx(this, vm => vm.Change);
        }

        #endregion
    }
}
