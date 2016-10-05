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
using Y_POS.Core.Infrastructure;

namespace Y_POS.Core.Checkout
{
    public sealed class CheckoutController : ReactiveObject
    {
        #region Fields

        private readonly IOrderService _orderService;
        private readonly ICheckoutService _checkoutService;
        private readonly IPaymentService _paymentService;

        #endregion

        #region Properties

        public Guid OrderId { get; private set; }
        public bool IsInitialized => OrderId != Guid.Empty;

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

        #region Constructor

        public CheckoutController(IOrderService orderService, 
            ICheckoutService checkoutService, 
            IPaymentService paymentService)
        {
            Guard.NotNull(orderService, nameof(orderService));
            Guard.NotNull(checkoutService, nameof(checkoutService));
            Guard.NotNull(paymentService, nameof(paymentService));

            _orderService = orderService;
            _checkoutService = checkoutService;
            _paymentService = paymentService;

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
            if (IsInitialized) throw new InvalidOperationException($"{nameof(CheckoutController)} already initialized!");

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
        public async Task SetDiscountAsync(string discountName, CancellationToken ct)
        {
            CheckInitialized();

            await Task.Delay(300, ct);

            CurrentDiscountName = discountName;
        }

        /*
         * PAYMENT
         */
        public async Task<IPaymentResponse> Pay(PaymentParams paymentParams)
        {
            Guard.NotNull(paymentParams, nameof(paymentParams));
            CheckInitialized();

            var result = await _paymentService.ProcessPayment(paymentParams).ToTask().ConfigureAwait(false);

            if (result.IsSuccess)
            {
                await RefreshReceipts(CancellationToken.None).ConfigureAwait(false);
            }
            return result;
        }

        #endregion

        #region Private methods

        private void CheckInitialized()
        {
            if (!IsInitialized) throw new InvalidOperationException($"{nameof(CheckoutController)} is not initialized!");
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
                    shouldRefreshReceipts = await _checkoutService.SplitReceiptsProportionally(OrderId, (int[]) args).ToTask(ct)
                        .ConfigureAwait(false);
                    SplittingType = SplittingType.SplitProportionally;
                    break;
            }

            if (!shouldRefreshReceipts || ct.IsCancellationRequested) return;

            await RefreshReceipts(ct).ConfigureAwait(false);
        }

        private async Task RefreshReceipts(CancellationToken ct)
        {
            var receipts = await _checkoutService.GetReceiptsByOrderId(OrderId).ToTask(ct).ConfigureAwait(false);

            if (ct.IsCancellationRequested) return;

            Receipts = receipts.Select(dto => new ReceiptItem(dto)).ToArray();
        }

        #endregion
    }
}
