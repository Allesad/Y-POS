using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Services;
using YumaPos.Shared.API.Enums;
using YumaPos.Shared.API.Models;
using Y_POS.Core.Infrastructure;

namespace Y_POS.Core.Checkout
{
    public sealed class CheckoutController : ReactiveObject
    {
        #region Fields

        private readonly IOrderService _orderService;
        private readonly ICheckoutService _checkoutService;
        private readonly IDiscountService _discountService;
        private readonly IPaymentService _paymentService;

        #endregion

        #region Properties

        public Guid OrderId { get; private set; }
        public bool IsInitialized => OrderId != Guid.Empty;
        public bool OrderIsPaid { get; private set; }

        public IObservable<ReceiptItem[]> ReceiptsStream { get; }
        public IObservable<OrderStatus> OrderStatusStream { get; }
        public IObservable<string> CustomerNameStream { get; }
        public IObservable<SplittingType> SplittingTypeStream { get; }
        public IObservable<string> DiscountStream { get; }

        [Reactive]
        private ReceiptItem[] Receipts { get; set; }

        [Reactive]
        private OrderStatus OrderStatus { get; set; }

        [Reactive]
        private string CustomerName { get; set; }

        [Reactive]
        private SplittingType SplittingType { get; set; }

        [Reactive]
        private string CurrentDiscountName { get; set; }

        #endregion

        #region Event

        public event EventHandler PaymentCompleted;

        #endregion

        #region Constructor

        public CheckoutController(IOrderService orderService,
            ICheckoutService checkoutService,
            IPaymentService paymentService,
            IDiscountService discountService)
        {
            Guard.NotNull(orderService, nameof(orderService));
            Guard.NotNull(checkoutService, nameof(checkoutService));
            Guard.NotNull(paymentService, nameof(paymentService));
            Guard.NotNull(discountService, nameof(discountService));

            _orderService = orderService;
            _checkoutService = checkoutService;
            _paymentService = paymentService;
            _discountService = discountService;

            ReceiptsStream = this.WhenAnyValue(controller => controller.Receipts);
            OrderStatusStream = this.WhenAnyValue(controller => controller.OrderStatus);
            CustomerNameStream = this.WhenAnyValue(controller => controller.CustomerName);
            SplittingTypeStream = this.WhenAnyValue(controller => controller.SplittingType);
            DiscountStream = this.WhenAnyValue(controller => controller.CurrentDiscountName);
        }

        #endregion

        #region Pubic methods

        public Task Init(Guid orderId)
        {
            return Init(orderId, CancellationToken.None);
        }

        public Task Init(Guid orderId, CancellationToken ct)
        {
            Guard.NonEmptyGuid(orderId, nameof(orderId));
            if (IsInitialized)
                throw new InvalidOperationException($"{nameof(CheckoutController)} already initialized!");

            return InitImpl(orderId, ct);
        }

        /*
         * SPLITTING
         * */

        public Task SplitAllOnOne()
        {
            return SplitAllOnOne(CancellationToken.None);
        }

        public Task SplitAllOnOne(CancellationToken ct)
        {
            CheckInitialized();

            return SplitInternal(SplittingType.AllOnOne, null, ct);
        }

        public Task SplitEvenly(int count)
        {
            return SplitEvenly(count, CancellationToken.None);
        }

        public Task SplitEvenly(int count, CancellationToken ct)
        {
            Guard.IsPositive(count, nameof(count));

            CheckInitialized();

            return SplitInternal(SplittingType.SplitEvently, count, ct);
        }

        public Task SplitProportionally(int[] proportions)
        {
            return SplitProportionally(proportions, CancellationToken.None);
        }

        public Task SplitProportionally(int[] proportions, CancellationToken ct)
        {
            Guard.NotEmpty(proportions, nameof(proportions));
            if (proportions.Sum() != 100)
            {
                throw new ArgumentException("Sum of proportions must be equals 100");
            }
            if (proportions.Any(i => i < 0))
            {
                throw new ArgumentException("Proportions cannot contain negative values");
            }

            CheckInitialized();

            return SplitInternal(SplittingType.SplitProportionally, proportions, ct);
        }

        /*
         * DISCOUNT
         * */

        public IObservable<DiscountDto[]> GetDiscountsAsync()
        {
            return GetDiscountsAsync(CancellationToken.None);
        }

        public IObservable<DiscountDto[]> GetDiscountsAsync(CancellationToken ct)
        {
            return _discountService.GetAllDiscounts();
        }

        public Task ApplyDiscountAsync(ReceiptItem receipt, DiscountDto discount)
        {
            return ApplyDiscountAsync(receipt, discount, CancellationToken.None);
        }

        public Task ApplyDiscountAsync(ReceiptItem receipt, DiscountDto discount, CancellationToken ct)
        {
            Guard.NotNull(receipt, nameof(receipt));
            Guard.NotNull(discount, nameof(discount));
            CheckInitialized();

            return ApplyDiscountInternal(receipt, discount, ct);
        }

        public Task RemoveAllDiscountsAsync()
        {
            return RemoveAllDiscountsAsync(CancellationToken.None);
        }

        public Task RemoveAllDiscountsAsync(CancellationToken ct)
        {
            CheckInitialized();

            return RemoveAllDiscountsInternal(ct);
        }

        /*
         * PAYMENT
         */

        public Task<IPaymentResponse> PayAsync(PaymentParams paymentParams)
        {
            return PayAsync(paymentParams, CancellationToken.None);
        }

        public Task<IPaymentResponse> PayAsync(PaymentParams paymentParams, CancellationToken ct)
        {
            Guard.NotNull(paymentParams, nameof(paymentParams));
            CheckInitialized();

            return PayInternal(paymentParams, ct);
        }

        public Task<IPaymentResponse> RefundAsync(ReceiptItem receiptToRefund)
        {
            return RefundAsync(receiptToRefund, CancellationToken.None);
        }

        public Task<IPaymentResponse> RefundAsync(ReceiptItem receiptToRefund, CancellationToken ct)
        {
            Guard.NotNull(receiptToRefund, nameof(receiptToRefund));
            Guard.IsTrue(receiptToRefund.IsPaid, nameof(receiptToRefund.IsPaid));
            Guard.IsFalse(receiptToRefund.IsRefunded, nameof(receiptToRefund.IsRefunded));
            CheckInitialized();

            return RefundInternal(receiptToRefund, ct);
        }

        /* 
         * PROGRESS
         */

        public Task StartOrder()
        {
            CheckInitialized();

            return UpdateOrderStatus(OrderStatus.InProgress);
        }

        public Task DoneOrder()
        {
            CheckInitialized();

            return UpdateOrderStatus(OrderStatus.Prepared);
        }

        /*
         * VOID
         */

        public Task VoidOrderAsync()
        {
            CheckInitialized();

            return OrderStatus != OrderStatus.Void
                ? UpdateOrderStatus(OrderStatus.Void)
                : Task.FromResult(0);
        }

        /* 
         * CLOSE
         */

        public Task CloseOrderAsync()
        {
            CheckInitialized();

            return OrderStatus != OrderStatus.Closed
                ? CloseInternal()
                : Task.FromResult(0);
        }

        #endregion

        #region Private methods

        private void CheckInitialized()
        {
            if (!IsInitialized)
                throw new InvalidOperationException($"{nameof(CheckoutController)} is not initialized!");
        }

        private async Task InitImpl(Guid orderId, CancellationToken ct)
        {
            OrderId = orderId;

            var orderTask = _orderService.GetOrderById(OrderId).ToTask(ct);
            var receiptsTask = _checkoutService.GetReceiptsByOrderId(OrderId).ToTask(ct);

            await Task.WhenAll(orderTask, receiptsTask).ConfigureAwait(false);

            if (ct.IsCancellationRequested) return;

            var order = orderTask.Result;
            var receipts = receiptsTask.Result;

            OrderStatus = order.Status;
            CustomerName = order.CustomerName;
            SplittingType = order.Splitting;
            Receipts = receipts.Select(dto => new ReceiptItem(dto, OrderStatus == OrderStatus.Void)).ToArray();
        }

        private async Task SplitInternal(SplittingType type, object args, CancellationToken ct)
        {
            bool shouldRefreshReceipts = false;

            switch (type)
            {
                case SplittingType.AllOnOne:
                    shouldRefreshReceipts = await _checkoutService.SplitAllReceiptsToOne(OrderId).ToTask(ct)
                        .ConfigureAwait(false);
                    SplittingType = SplittingType.AllOnOne;
                    break;
                case SplittingType.SplitEvently:
                    shouldRefreshReceipts = await _checkoutService.SplitReceiptsEvenly(OrderId, (int) args).ToTask(ct)
                        .ConfigureAwait(false);
                    SplittingType = SplittingType.SplitEvently;
                    break;
                case SplittingType.SplitProportionally:
                    shouldRefreshReceipts =
                        await _checkoutService.SplitReceiptsProportionally(OrderId, (int[]) args).ToTask(ct)
                            .ConfigureAwait(false);
                    SplittingType = SplittingType.SplitProportionally;
                    break;
            }

            if (!shouldRefreshReceipts || ct.IsCancellationRequested) return;

            await RefreshReceipts(ct).ConfigureAwait(false);
        }

        private async Task ApplyDiscountInternal(ReceiptItem receipt, DiscountDto discount, CancellationToken ct)
        {
            var response =
                await _checkoutService.AddDiscountToReceipt(OrderId, discount.Id, receipt.SplittingNumber).ToTask(ct)
                    .ConfigureAwait(false);

            if (response.Errors == null)
            {
                await RefreshReceipts(ct).ConfigureAwait(false);
            }
            CurrentDiscountName = discount.Name;
        }

        private async Task RemoveAllDiscountsInternal(CancellationToken ct)
        {
            var response = await _checkoutService.RemoveAllDiscounts(OrderId).ToTask(ct).ConfigureAwait(false);

            if (response.Errors == null)
            {
                await RefreshReceipts(ct).ConfigureAwait(false);
            }
        }

        private async Task<IPaymentResponse> PayInternal(PaymentParams paymentParams, CancellationToken ct)
        {
            var result = await _paymentService.ProcessPayment(paymentParams).ToTask(ct).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                await RefreshReceipts(ct).ConfigureAwait(false);

                if (OrderStatus != OrderStatus.Closed && OrderStatus != OrderStatus.Void &&
                    Receipts.All(item => item.IsPaid))
                {
                    RaisePaymentCompletedEvent();
                }
            }
            return result;
        }

        private async Task<IPaymentResponse> RefundInternal(ReceiptItem receiptToRefund, CancellationToken ct)
        {
            var result = await _paymentService.ProcessRefund(new PaymentParams
            {
                OrderId = OrderId,
                ReceiptNumber = receiptToRefund.SplittingNumber
            }).ToTask(ct).ConfigureAwait(false);

            if (result.IsSuccess)
            {
                await RefreshReceipts(ct).ConfigureAwait(false);
            }
            return result;
        }

        private async Task UpdateOrderStatus(OrderStatus status)
        {
            await _orderService.UpdateOrderStatus(OrderId, (int) status);
            OrderStatus = status;
        }

        private async Task CloseInternal()
        {
            await _orderService.CloseOrder(OrderId).ToTask().ConfigureAwait(false);
            OrderStatus = OrderStatus.Closed;
        }

        private async Task RefreshReceipts(CancellationToken ct)
        {
            var receipts = await _checkoutService.GetReceiptsByOrderId(OrderId).ToTask(ct).ConfigureAwait(false);

            if (ct.IsCancellationRequested) return;

            Receipts = receipts.Select(dto => new ReceiptItem(dto)).ToArray();
            OrderIsPaid = Receipts.All(item => item.IsPaid);
        }

        private void RaisePaymentCompletedEvent()
        {
            var handler = Volatile.Read(ref PaymentCompleted);
            handler?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}