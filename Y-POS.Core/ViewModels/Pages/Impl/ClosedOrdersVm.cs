using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Common;
using YumaPos.Client.Services;
using YumaPos.Shared.API.Models;
using Y_POS.Core.Extensions;
using Y_POS.Core.Infrastructure;
using Y_POS.Core.Properties;
using Y_POS.Core.ViewModels.Dialogs;
using Y_POS.Core.ViewModels.Items.Impl;

namespace Y_POS.Core.ViewModels.Pages
{
    public sealed class ClosedOrdersVm : PosPageVm, IClosedOrdersVm
    {
        #region Fields

        private readonly IOrderService _orderService;

        private readonly IObservable<FilteredRestaurantOrdersDto> _closedOrdersStream;

        private ReactiveCommand<object> _commandPrevPage;
        private ReactiveCommand<object> _commandNextPage;
        private ReactiveCommand<object> _commandFirstPage;
        private ReactiveCommand<object> _commandLastPage;
        private ReactiveCommand<object> _commandItemsPerPage;
        private ReactiveCommand<object> _commandShowTransactions;

        #endregion

        #region Properties

        [Reactive]
        public ClosedOrderItemVm[] Items { get; private set; }

        [Reactive]
        public ClosedOrderItemVm SelectedItem { get; set; }

        [Reactive]
        public string SearchText { get; set; }

        [Reactive]
        public int PageNumber { get; private set; }
        public extern int LastPageNumber { [ObservableAsProperty] get; }

        [Reactive]
        public int PageSize { get; set; }

        [Reactive]
        public DateTime? DateStart { get; set; }

        [Reactive]
        public DateTime? DateEnd { get; set; }

        [Reactive]
        private int OrdersCount { get; set; }

        #endregion]

        #region Commands

        public ICommand CommandPrevPage => _commandPrevPage;
        public ICommand CommandNextPage => _commandNextPage;
        public ICommand CommandFirstPage => _commandFirstPage;
        public ICommand CommandLastPage => _commandLastPage;
        public ICommand CommandItemsPerPage => _commandItemsPerPage;
        public ICommand CommandShowTransactions => _commandShowTransactions;

        #endregion

        #region Constructor

        public ClosedOrdersVm(IOrderService orderService)
        {
            if (orderService == null) throw new ArgumentNullException(nameof(orderService));

            _orderService = orderService;

            _closedOrdersStream = ClosedOrdersStream();
        }

        #endregion

        #region Lifecycle

        protected override void OnCreate(IArgsBundle args)
        {
            PageNumber = 1;
            PageSize = 10;
            SearchText = string.Empty;

            this.WhenAny(vm => vm.OrdersCount, vm => vm.PageSize,
                    (count, size) => count.Value == 0
                        ? 1
                        : (int) Math.Ceiling(count.Value / (double) size.Value))
                .ToPropertyEx(this, vm => vm.LastPageNumber);
        }

        protected override void InitCommands()
        {
            var canExecuteFirstOrPrevPage = this.WhenAnyValue(vm => vm.PageNumber).Select(pageNumber => pageNumber > 1);
            var canExecuteLastOrNextPage = this.WhenAny(vm => vm.PageNumber, vm => vm.LastPageNumber,
                vm => vm.OrdersCount, vm => vm.PageSize,
                (pageNumber, lastPageNumber, ordersCount, pageSize) => pageNumber.Value < lastPageNumber.Value && ordersCount.Value > pageSize.Value);
            var canExecuteShowTransactions =
                this.WhenAnyValue(vm => vm.SelectedItem).Select(order => order != null && order.HasMultipleTransactions);

            _commandFirstPage = ReactiveCommand.Create(canExecuteFirstOrPrevPage);
            _commandPrevPage = ReactiveCommand.Create(canExecuteFirstOrPrevPage);
            _commandNextPage = ReactiveCommand.Create(canExecuteLastOrNextPage);
            _commandLastPage = ReactiveCommand.Create(canExecuteLastOrNextPage);
            _commandItemsPerPage = ReactiveCommand.Create();
            _commandShowTransactions = ReactiveCommand.Create(canExecuteShowTransactions);
        }

        protected override void InitLifetimeSubscriptions()
        {
            AddLifetimeSubscription(_commandFirstPage.Subscribe(_ => PageNumber = 1));
            AddLifetimeSubscription(_commandPrevPage.Subscribe(_ => PageNumber--));
            AddLifetimeSubscription(_commandNextPage.Subscribe(_ => PageNumber++));
            AddLifetimeSubscription(_commandLastPage.Subscribe(_ => PageNumber = LastPageNumber));
            AddLifetimeSubscription(_commandShowTransactions
                .Select(_ => new {SelectedItem.Transactions, SelectedItem.OrderNumber})
                .Subscribe(args => ShowTransactionsDialog(args.Transactions, args.OrderNumber)));
            AddLifetimeSubscription(_commandItemsPerPage.Select(param => (int) param)
                .SubscribeToObserveOnUi(itemsPerPage =>
                {
                    Guard.IsPositive(itemsPerPage, nameof(itemsPerPage), "Items per page count cannot be negative!");

                    PageSize = itemsPerPage;
                }));
        }

        protected override void OnStart()
        {
            var stream = _closedOrdersStream.Publish();

            AddOneTimeSubsciption(stream.Select(dto => dto.Results)
                .Select(dtos => dtos.Select(dto => new ClosedOrderItemVm(dto)).ToArray())
                .SubscribeToObserveOnUi(orders => Items = orders, HandleError));
            AddOneTimeSubsciption(stream.Select(dto => dto.Count ?? 0)
                .SubscribeToObserveOnUi(count => OrdersCount = count, ex => {/* Skip error handling here since it's the same exception as above */ }));

            AddOneTimeSubsciption(stream.Connect());
        }

        #endregion

        #region Private methods

        private IObservable<FilteredRestaurantOrdersDto> ClosedOrdersStream()
        {
            return this.WhenAny(vm => vm.PageNumber, vm => vm.PageSize, vm => vm.SearchText, vm => vm.DateStart, vm => vm.DateEnd,
                    (pageNumber, pageSize, query, dateStart, dateEnd) => new
                    {
                        Number = pageNumber.Value,
                        Size = pageSize.Value,
                        Search = query.Value,
                        DateStart = dateStart.Value,
                        DateEnd = dateEnd.Value
                    })
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Select(args => _orderService.GetClosedOrders(
                        (args.Number - 1) * args.Size,
                        args.Size,
                        args.DateStart, args.DateEnd,
                        args.Search
                    ))
                .Switch();
        }

        private void ShowTransactionsDialog(IEnumerable<OrderTransactionItemVm> transactions, int orderNumber = -1)
        {
            DialogService.CreateCustomDialog(
                new OrderTransactionsViewerDialog(transactions),
                orderNumber != -1 ? string.Format(Resources.Placeholder_TransactionsForOrder, orderNumber) : string.Empty).Show();
        }

        #endregion
    }
}
