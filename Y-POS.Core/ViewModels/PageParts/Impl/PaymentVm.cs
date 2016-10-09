using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using DialogManagement.Contracts;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Services;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Common.Infrastructure.BusinessLogic.Tendering;
using Y_POS.Core.Checkout;
using Y_POS.Core.Extensions;
using Y_POS.Core.Infrastructure;
using Y_POS.Core.ViewModels.Dialogs;
using Y_POS.Core.ViewModels.Pages;

namespace Y_POS.Core.ViewModels.PageParts
{
    public class PaymentVm : BaseVm, IPaymentVm
    {
        #region Fields

        private readonly CheckoutController _controller;
        private readonly SelectedReceiptController _selectedReceiptController;
        
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

        public extern bool IsMultiplePayment { [ObservableAsProperty] get; }

        #endregion

        #region Commands

        public ICommand CommandCheckout { get; }
        public ICommand CommandAddPayment { get; }
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

            var cmdCheckout = ReactiveCommand.CreateAsyncTask(canPayStream, 
                (_, ct) => Pay(_controller.OrderId, _selectedReceiptController.CurrentReceipt.SplittingNumber, ct));
            cmdCheckout
                .SubscribeToObserveOnUi(response =>
                {
                    if (!response.IsSuccess)
                    {
                        DialogService.ShowErrorMessage(response.ErrorMessage);
                        return;
                    }
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

            this.WhenAnyValue(vm => vm.PaymentType)
                .Select(type => type == PaymentType.Multiple)
                .ToPropertyEx(this, vm => vm.IsMultiplePayment, false, SchedulerService.UiScheduler);

            // Command Add payment
            //var cmdAddPayment = ReactiveCommand.CreateAsyncTask();
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

        private async Task<TenderParams[]> GetPaymentInfo()
        {
            switch (PaymentType)
            {
                case PaymentType.Cash:
                    return new[] {new TenderParams {Amount = Received, TipAmount = Tips, TenderType = TenderType.Ca}};
                case PaymentType.Card:
                    return new[] {new TenderParams {Amount = Received, TipAmount = Tips, TenderType = TenderType.Cc}};
                case PaymentType.GiftCard:
                    var dialog = new GiftCardNumberDialog();
                    if (await DialogService.CreateCustomDialog(dialog, "GIFT CARD").ShowAsync() == DialogButtonType.Cancel)
                    {
                        return null;
                    }
                    return new[] {new TenderParams
                    {
                        Amount = Received,
                        TipAmount = Tips,
                        TenderType = TenderType.Eg,
                        Ccnum = dialog.CardNumber
                    }};
            }
            throw new InvalidOperationException("Selected payment type not supported!");
        }

        private async Task<ResponseMessage> Pay(Guid orderId, int splittingNumber, CancellationToken ct)
        {
            var tenders = await GetPaymentInfo().ConfigureAwait(false);

            if (ct.IsCancellationRequested)
            {
                return ResponseMessage.Fail("Operation cancelled");
            }

            if (tenders == null)
            {
                return ResponseMessage.Success();
            }
            
            var paymentResponse = await _controller.PayAsync(new PaymentParams
            {
                OrderId = orderId,
                ReceiptNumber = splittingNumber,
                Tenders = tenders
            }, ct).ConfigureAwait(false);

            return paymentResponse.IsSuccess
                ? ResponseMessage.Success()
                : ResponseMessage.Fail(paymentResponse.Message);
        } 

        private void Reset()
        {
            Subtotal = Total = Received = Tips = 0;
        }

        #endregion
    }
}
