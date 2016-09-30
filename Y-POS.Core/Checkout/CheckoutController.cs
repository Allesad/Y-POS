using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Services;
using YumaPos.Shared.API.Enums;
using Y_POS.Core.Infrastructure;

namespace Y_POS.Core.Checkout
{
    public sealed class CheckoutController
    {
        #region Fields

        private readonly IOrderService _orderService;
        private readonly ICheckoutService _checkoutService;
        private readonly IPaymentService _paymentService;

        #endregion

        #region Properties

        public Guid OrderId { get; private set; }

        public IObservable<ReceiptItem[]> ReceiptsStream { get; }
        public IObservable<OrderStatus> OrderStatusStream { get; }
        public IObservable<string> CustomerNameStream { get; }
        public IObservable<SplittingType> SplittingTypeStream { get; }
        
        [Reactive]
        private ReceiptItem[] Receipts { get; set; }

        [Reactive]
        private OrderStatus OrderStatus { get; set; }

        [Reactive]
        private string CustomerName { get; set; }

        [Reactive]
        private SplittingType SplittingType { get; set; }

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
        }

        #endregion

        #region Pubic methods

        public Task Init(Guid orderId)
        {
            Guard.NonEmptyGuid(orderId, nameof(orderId));

            return InitImpl(orderId);
        }

        public Task SplitAllOnOne()
        {
            CheckInitialized();

            return _checkoutService.SplitAllReceiptsToOne(OrderId).ToTask();
        }

        public Task SplitEvenly(int count)
        {
            Guard.IsPositive(count, nameof(count));

            CheckInitialized();

            return _checkoutService.SplitReceiptsEvenly(OrderId, count).ToTask();
        }

        public Task SplitProportionally(int[] proportions)
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

            return _checkoutService.SplitReceiptsProportionally(OrderId, proportions).ToTask();
        }

        #endregion

        #region Private methods

        private void CheckInitialized()
        {
            if (OrderId == Guid.Empty) throw new InvalidOperationException($"{nameof(CheckoutController)} is not initialized!");
        }

        private async Task InitImpl(Guid orderId)
        {
            OrderId = orderId;

            var orderTask = _orderService.GetOrderById(OrderId).ToTask();
            var receiptsTask = _checkoutService.GetReceiptsByOrderId(OrderId).ToTask();

            await Task.WhenAll(orderTask, receiptsTask).ConfigureAwait(false);

            var order = orderTask.Result;
            var receipts = receiptsTask.Result;

            OrderStatus = order.Status;
            CustomerName = order.CustomerName;
            SplittingType = order.Splitting;
        }

        private async Task SplitInternal(SplittingType type, object args)
        {
            bool shouldRefreshReceipts = false;

            switch (type)
            {
                case SplittingType.AllOnOne:
                    shouldRefreshReceipts = await _checkoutService.SplitAllReceiptsToOne(OrderId).ToTask()
                        .ConfigureAwait(false);
                    SplittingType = SplittingType.AllOnOne;
                    break;
                case SplittingType.SplitEvently:
                    shouldRefreshReceipts = await _checkoutService.SplitReceiptsEvenly(OrderId, (int) args).ToTask()
                        .ConfigureAwait(false);
                    SplittingType = SplittingType.SplitEvently;
                    break;
                case SplittingType.SplitProportionally:
                    shouldRefreshReceipts = await _checkoutService.SplitReceiptsProportionally(OrderId, (int[]) args).ToTask()
                        .ConfigureAwait(false);
                    SplittingType = SplittingType.SplitProportionally;
                    break;
            }

            if (!shouldRefreshReceipts) return;

            await RefreshReceipts();
        }

        private async Task RefreshReceipts()
        {
            var receipts = await _checkoutService.GetReceiptsByOrderId(OrderId).ToTask().ConfigureAwait(false);


        }

        #endregion
    }
}
