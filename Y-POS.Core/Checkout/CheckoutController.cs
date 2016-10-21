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
using YumaPos.Shared.Core.Utils.Formating;
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

        [Reactive]
        public bool OrderIsPaid { get; private set; }

        public IObservable<ReceiptItem[]> ReceiptsStream { get; }
        public IObservable<OrderStatus> OrderStatusStream { get; }
        public IObservable<string> CustomerNameStream { get; }
        public IObservable<SplittingType> SplittingTypeStream { get; }
        public IObservable<string> DiscountStream { get; }

        [Reactive]
        private ReceiptItem[] Receipts { get; set; }

        [Reactive]
        public OrderStatus OrderStatus { get; private set; }

        [Reactive]
        private string CustomerName { get; set; }

        [Reactive]
        public string EmployeeName { get; private set; }

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

        public Task InitAsync(Guid orderId)
        {
            return InitAsync(orderId, CancellationToken.None);
        }

        public Task InitAsync(Guid orderId, CancellationToken ct)
        {
            Guard.NonEmptyGuid(orderId, nameof(orderId));
            if (IsInitialized)
                throw new InvalidOperationException($"{nameof(CheckoutController)} already initialized!");

            return InitImpl(orderId, ct);
        }

        /* 
         * CUSTOMERS
         */
        public Task SetCustomerAsync(CustomerDto customer)
        {
            return SetCustomerAsync(customer, CancellationToken.None);
        }

        public Task SetCustomerAsync(CustomerDto customer, CancellationToken ct)
        {
            Guard.NotNull(customer, nameof(customer));
            Guard.NotNull(customer.CustomerId, nameof(customer.CustomerId));
            CheckInitialized();

            return SetCustomerInternal(customer, ct);
        }

        /*
         * SPLITTING
         * */

        public Task SplitAllOnOneAsync()
        {
            return SplitAllOnOneAsync(CancellationToken.None);
        }

        public Task SplitAllOnOneAsync(CancellationToken ct)
        {
            CheckInitialized();

            return SplitInternal(SplittingType.AllOnOne, null, ct);
        }

        public Task SplitEvenlyAsync(int count)
        {
            return SplitEvenlyAsync(count, CancellationToken.None);
        }

        public Task SplitEvenlyAsync(int count, CancellationToken ct)
        {
            Guard.IsPositive(count, nameof(count));

            CheckInitialized();

            return SplitInternal(SplittingType.SplitEvently, count, ct);
        }

        public Task SplitProportionallyAsync(int[] proportions)
        {
            return SplitProportionallyAsync(proportions, CancellationToken.None);
        }

        public Task SplitProportionallyAsync(int[] proportions, CancellationToken ct)
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

        public Task StartOrderAsync()
        {
            CheckInitialized();

            return UpdateOrderStatus(OrderStatus.InProgress);
        }

        public Task DoneOrderAsync()
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

            await Task.WhenAll(orderTask, RefreshReceipts(ct)).ConfigureAwait(false);

            if (ct.IsCancellationRequested) return;

            var order = orderTask.Result;

            OrderStatus = order.Status;
            if (order.CustomerDto != null)
            {
                CustomerName = FormattingUtils.FullName(order.CustomerDto.FirstName, order.CustomerDto.LastName);
            }
            if (order.EmployeeDto != null)
            {
                EmployeeName = FormattingUtils.FullName(order.EmployeeDto.FirstName, order.EmployeeDto.LastName);
            }
            SplittingType = order.Splitting;
        }

        private async Task SetCustomerInternal(CustomerDto customer, CancellationToken ct)
        {
            // ReSharper disable once PossibleInvalidOperationException
            await _orderService.UpdateOrderCustomer(OrderId, customer.CustomerId.Value).ToTask(ct).ConfigureAwait(false);
            
            CustomerName = FormattingUtils.FullName(customer.FirstName, customer.LastName);
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

            var isVoid = OrderStatus == OrderStatus.Void;
            Receipts = receipts.Select(dto => new ReceiptItem(dto, isVoid)).ToArray();
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