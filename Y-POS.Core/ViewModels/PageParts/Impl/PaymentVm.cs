using System;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Services;
using YumaPos.Client.UI.ViewModels.Impl;
using Y_POS.Core.Checkout;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Pages;

namespace Y_POS.Core.ViewModels.PageParts
{
    public class PaymentVm : BaseVm, IPaymentVm
    {
        #region Fields

        private readonly CheckoutController _controller;
        private readonly SelectedReceiptController _selectedReceiptController;

        private PaymentParams _currentPaymentParams;

        #endregion

        #region Properties

        [Reactive]
        public PaymentType PaymentType { get; private set; }

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
        public ICommand CommandSetPaymentType { get; }

        #endregion

        #region Constructor

        public PaymentVm(CheckoutController controller, SelectedReceiptController selectedReceiptController)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            if (selectedReceiptController == null) throw new ArgumentNullException(nameof(selectedReceiptController));

            _controller = controller;
            _selectedReceiptController = selectedReceiptController;

            var canPayStream = this.WhenAny(vm => vm.Total, vm => vm.Received, vm => vm.Tips,
                    (total, received, tips) => (received.Value - total.Value - tips.Value) >= 0)
                .CombineLatest(_selectedReceiptController.SelectedReceiptStream.Select(item => item != null), 
                    (paymentValid, hasSelectedReceipt) => paymentValid && hasSelectedReceipt);

            var cmdCheckout = ReactiveCommand.CreateAsyncTask(canPayStream, (_, ct) => _controller.Pay(_currentPaymentParams));
            cmdCheckout
                .Subscribe(response =>
                {
                    if (!response.IsSuccess)
                    {
                        DialogService.ShowErrorMessage(response.Message);
                    }
                    /*DialogService.ShowNotificationMessage($"Received: {Received.ToString("c")},\n" +
                                                          $"Tips: {Tips.ToString("c")}");*/
                });

            CommandCheckout = cmdCheckout;

            this.WhenAnyObservable(vm => vm._selectedReceiptController.SelectedReceiptStream)
                .SubscribeToObserveOnUi(OnNewSelectedReceipt);

            this.WhenAny(vm => vm.Total, vm => vm.Received, vm => vm.Tips,
                (total, received, tips) => received.Value - total.Value - tips.Value)
                .Select(leftover => Math.Max(leftover, 0))
                .ToPropertyEx(this, vm => vm.Change);

            var cmdSetPaymentType = ReactiveCommand.Create();
            cmdSetPaymentType.Select(param => (PaymentType) param)
                .Subscribe(type => PaymentType = type);

            CommandSetPaymentType = cmdSetPaymentType;
        }

        #endregion

        #region Private methods

        private void OnNewSelectedReceipt(ReceiptItem receipt)
        {
            if (receipt == null)
            {
                Reset();
                return;
            }
            Subtotal = receipt.Subtotal;
            Total = receipt.Total;
        }

        private void Reset()
        {
            Subtotal = Total = Received = Tips = 0;
        }

        #endregion
    }
}
