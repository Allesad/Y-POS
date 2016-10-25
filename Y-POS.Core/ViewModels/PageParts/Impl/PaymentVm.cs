using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using DialogManagement.Contracts;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Services;
using YumaPos.Common.Infrastructure.BusinessLogic.Tendering;
using Y_POS.Core.Checkout;
using Y_POS.Core.Enums;
using Y_POS.Core.Extensions;
using Y_POS.Core.Infrastructure;
using Y_POS.Core.Infrastructure.Exceptions;
using Y_POS.Core.Properties;
using Y_POS.Core.ViewModels.Dialogs;

namespace Y_POS.Core.ViewModels.PageParts
{
    public class PaymentVm : PosBaseVm, IPaymentVm
    {
        public enum PaymentMode
        {
            /// <summary>
            /// Default mode for editable inputs and keypad
            /// </summary>
            Default,

            /// <summary>
            /// Summary overview with disabled inputs for multiple payment
            /// </summary>
            MultiPaymentOverview
        }

        #region Fields

        private readonly CheckoutController _controller;
        private readonly SelectedReceiptController _selectedReceiptController;

        private PaymentType _currentPaymentType;
        private PaymentParams _paymentParams;
        
        #endregion

        #region Properties

        [Reactive]
        public PaymentMode Mode { get; private set; }

        [Reactive]
        public decimal Received { get; set; }

        [Reactive]
        public decimal Tips { get; set; }

        [Reactive]
        public decimal Subtotal { get; private set; }

        [Reactive]
        public decimal Total { get; private set; }
        
        public extern decimal Change { [ObservableAsProperty] get; }

        [Reactive]
        public bool IsMultiplePayment { get; private set; }

        /* For multiple payment */
        [Reactive]
        public decimal TotalReceived { get; private set; }
        [Reactive]
        public decimal TotalTips { get; private set; }
        [Reactive]
        public decimal TotalCashPayment { get; private set; }
        [Reactive]
        public decimal TotalCardPayment { get; private set; }
        [Reactive]
        public decimal TotalGiftCardPayment { get; private set; }
        [Reactive]
        public decimal TotalMobilePayment { get; private set; }
        [Reactive]
        public decimal TotalPointsPayment { get; private set; }

        private PaymentParams PaymentParams
        {
            get
            {
                _paymentParams = _paymentParams ?? new PaymentParams
                {
                    OrderId = _controller.OrderId,
                    ReceiptNumber = _selectedReceiptController.CurrentReceipt?.SplittingNumber ?? -1,
                    Tenders = new List<TenderParams>(5)
                };
                return _paymentParams;
            }
        }

        #endregion

        #region Commands

        public ICommand CommandCheckout { get; }
        public ICommand CommandSetMultiplePayment { get; }
        public ICommand CommandSetPaymentType { get; }
        public ICommand CommandAddPartialPayment { get; }
        public ICommand CommandCancelPartialPayment { get; }
        public ICommand CommandResetMultiplePayment { get; }
        public ICommand CommandSubmitPartialPayment { get; }

        #endregion

        #region Constructor

        public PaymentVm(CheckoutController controller, SelectedReceiptController selectedReceiptController)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            if (selectedReceiptController == null) throw new ArgumentNullException(nameof(selectedReceiptController));

            _controller = controller;
            _selectedReceiptController = selectedReceiptController;

            this.WhenAnyObservable(vm => vm._selectedReceiptController.SelectedReceiptStream)
                .SubscribeToObserveOnUi(OnNewSelectedReceipt);

            this.WhenAny(vm => vm.Total, vm => vm.TotalReceived, vm => vm.TotalTips,
                (total, received, tips) => received.Value - total.Value - tips.Value)
                .Select(leftover => Math.Max(leftover, 0))
                .ToPropertyEx(this, vm => vm.Change);

            this.WhenAnyValue(vm => vm.Received)
                .Where(_ => !IsMultiplePayment)
                .SubscribeToObserveOnUi(received => TotalReceived = received);

            this.WhenAnyValue(vm => vm.Tips)
                .Where(_ => !IsMultiplePayment)
                .SubscribeToObserveOnUi(tips => TotalTips = tips);

            // Create commands
            var cmdSetMultiplePayment = ReactiveCommand.Create();
            var cmdSetPaymentType = ReactiveCommand.Create();
            var cmdAddPartialPayment = ReactiveCommand.Create();
            var cmdCancelPartialPayment = ReactiveCommand.Create();
            var cmdResetMultiplePayment = ReactiveCommand.Create();
            var cmdSubmitPartialPayment =
                ReactiveCommand.CreateAsyncTask(this.WhenAny(vm => vm.Mode, vm => vm.Received, vm => vm.Tips,
                    (mode, received, tips) => mode.Value == PaymentMode.Default && received.Value >= tips.Value),
                    _ => GetTenderParams(_currentPaymentType, Received, Tips));
            var cmdCheckout = ReactiveCommand.CreateAsyncTask(GetPayCanExecute(), async (_, ct) =>
                {
                    if (IsMultiplePayment) return await Pay(PaymentParams, ct);

                    var tenderParams = await GetTenderParams(_currentPaymentType, Received, Tips).ConfigureAwait(false);
                    if (tenderParams == null) return ResponseMessage.Fail(string.Empty);

                    PaymentParams.Tenders.Add(tenderParams);
                    return await Pay(PaymentParams, ct);
                });

            // Subscribe commands
            cmdSetMultiplePayment.Select(param => (bool)param).SubscribeToObserveOnUi(isMultiple =>
            {
                Reset();
                IsMultiplePayment = isMultiple;
                Mode = isMultiple ? PaymentMode.MultiPaymentOverview : PaymentMode.Default;
            });
            cmdSetPaymentType.Select(param => (PaymentType) param)
                .Subscribe(paymentType => _currentPaymentType = paymentType);
            cmdAddPartialPayment.Select(param => (PaymentType) param).SubscribeToObserveOnUi(type =>
            {
                _currentPaymentType = type;
                Mode = PaymentMode.Default;
            });
            cmdCancelPartialPayment.SubscribeToObserveOnUi(_ =>
            {
                Received = 0;
                Tips = 0;
                Mode = PaymentMode.MultiPaymentOverview;
            });
            cmdResetMultiplePayment.SubscribeToObserveOnUi(_ => Reset());
            cmdSubmitPartialPayment.Where(tenderParams => tenderParams != null).SubscribeToObserveOnUi(OnAddTenderParams);
            cmdCheckout.Subscribe(OnPaymentComplete);
            cmdCheckout.ThrownExceptions.Subscribe(HandleError);

            // Assign commands
            CommandSetMultiplePayment = cmdSetMultiplePayment;
            CommandSetPaymentType = cmdSetPaymentType;
            CommandAddPartialPayment = cmdAddPartialPayment;
            CommandCancelPartialPayment = cmdCancelPartialPayment;
            CommandSubmitPartialPayment = cmdSubmitPartialPayment;
            CommandResetMultiplePayment = cmdResetMultiplePayment;
            CommandCheckout = cmdCheckout;
        }

        #endregion

        #region Overridden methods

        protected override void HandleError(Exception exception)
        {
            if (!(exception is ServerRuntimeException)) throw exception;
            Logger.Error(exception.Message, exception);
            DialogService.ShowErrorMessage(Resources.Dialog_Error_ServerRuntimeError);
        }

        #endregion

        #region Private methods

        private IObservable<bool> GetPayCanExecute()
        {
            return this.WhenAny(vm => vm.Total, vm => vm.TotalReceived, vm => vm.TotalTips,
                    (total, received, tips) => (received.Value - total.Value - tips.Value) >= 0)
                .CombineLatest(_selectedReceiptController.SelectedReceiptStream,
                    (paymentValid, selectedReceipt) => paymentValid && selectedReceipt != null && !selectedReceipt.IsPaid);
        } 

        private void OnNewSelectedReceipt(ReceiptItem receipt)
        {
            if (receipt == null)
            {
                Reset();
                PaymentParams.ReceiptNumber = -1;
                return;
            }
            Subtotal = receipt.Subtotal;
            Total = receipt.Total;
            PaymentParams.ReceiptNumber = receipt.SplittingNumber;
        }

        private async Task<TenderParams> GetTenderParams(PaymentType paymentType, decimal received, decimal tips)
        {
            switch (paymentType)
            {
                case PaymentType.Cash:
                    return new TenderParams {Amount = received, TipAmount = tips, TenderType = TenderType.Ca};
                case PaymentType.Card:
                    return new TenderParams {Amount = received, TipAmount = tips, TenderType = TenderType.Cc};
                case PaymentType.GiftCard:
                    using (var dialog = new GiftCardNumberDialog())
                    {
                        if (await DialogService.CreateCustomDialog(dialog).ShowAsync() == DialogButtonType.Cancel)
                        {
                            return null;
                        }
                        return new TenderParams
                        {
                            Amount = received,
                            TipAmount = tips,
                            TenderType = TenderType.Eg,
                            Ccnum = dialog.CardNumber
                        };
                    }
                default:
                    throw new InvalidOperationException("Payment type not supported!");
            }
        }

        private void OnAddTenderParams(TenderParams tenderParams)
        {
            Guard.NotNull(tenderParams, nameof(tenderParams));
            
            _paymentParams.Tenders.Add(tenderParams);
            TotalReceived += Received;
            TotalTips += Tips;
            switch (_currentPaymentType)
            {
                case PaymentType.Cash:
                    TotalCashPayment += Received + Tips;
                    break;
                case PaymentType.Card:
                    TotalCardPayment += Received + Tips;
                    break;
                case PaymentType.GiftCard:
                    TotalGiftCardPayment += Received + Tips;
                    break;
                case PaymentType.Mobile:
                    TotalMobilePayment += Received + Tips;
                    break;
                case PaymentType.Points:
                    TotalPointsPayment += Received + Tips;
                    break;
            }
            Received = 0;
            Tips = 0;
            Mode = PaymentMode.MultiPaymentOverview;
        }

        private Task<ResponseMessage> Pay(PaymentParams paymentParams, CancellationToken ct)
        {
            Guard.NotNull(paymentParams, nameof(paymentParams));

            return PayImpl(paymentParams, ct);
        }

        private async Task<ResponseMessage> PayImpl(PaymentParams paymentParams, CancellationToken ct)
        {
            var paymentResponse = await _controller.PayAsync(paymentParams, ct).ConfigureAwait(false);

            return paymentResponse.IsSuccess
                ? ResponseMessage.Success()
                : ResponseMessage.Fail(paymentResponse.Message);
        }

        private void OnPaymentComplete(ResponseMessage result)
        {
            if (result.IsSuccess)
            {
                Reset();
                return;
            }
            if (!IsMultiplePayment)
            {
                PaymentParams.Tenders.Clear();
            }
            if (!result.ErrorMessage.IsEmpty())
            {
                DialogService.ShowErrorMessage(result.ErrorMessage);
            }
        }

        private void Reset()
        {
            Received = Tips = TotalCashPayment 
                = TotalCardPayment = TotalGiftCardPayment 
                = TotalMobilePayment = TotalPointsPayment = 0;
            TotalReceived = 0;
            TotalTips = 0;
            PaymentParams.Tenders.Clear();
        }

        #endregion
    }
}
