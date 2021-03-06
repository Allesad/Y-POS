﻿using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Navigation;
using YumaPos.Shared.API.Enums;
using Y_POS.Core.Checkout;
using Y_POS.Core.Extensions;
using Y_POS.Core.Infrastructure.Exceptions;
using Y_POS.Core.Properties;

namespace Y_POS.Core.ViewModels.PageParts
{
    public enum OrderProgressState
    {
        NotStarted,
        InProgress,
        Prepared
    }

    public sealed class PaymentCompleteVm : PosBaseVm, IPaymentCompleteVm
    {
        #region Fields

        private readonly CheckoutController _controller;
        private readonly ReactiveCommand<Unit> _commandCloseOrder;
        private readonly ReactiveCommand<Unit> _commandCloseAndCreateNew;
        private readonly ReactiveCommand<object> _commandNewOrder;
        private readonly ReactiveCommand<object> _commandActiveOrders;
        private readonly ReactiveCommand<Unit> _commandStart;
        private readonly ReactiveCommand<Unit> _commandDone;

        #endregion

        #region Properties
        
        public extern OrderProgressState ProgressState { [ObservableAsProperty] get; }

        public decimal OrderAmount { get; set; }
        public string EmployeeName => _controller.EmployeeName;

        #endregion

        #region Commands

        public ICommand CommandCloseOrder => _commandCloseOrder;
        public ICommand CommandCloseAndCreateNewOrder => _commandCloseAndCreateNew;
        public ICommand CommandNewOrder => _commandNewOrder;
        public ICommand CommandActiveOrders => _commandActiveOrders;
        public ICommand CommandStart => _commandStart;
        public ICommand CommandDone => _commandDone;

        #endregion

        #region Constructor

        public PaymentCompleteVm(CheckoutController controller)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));

            _controller = controller;

            // Set up commands
            _commandCloseOrder = ReactiveCommand.CreateAsyncTask(_ => _controller.CloseOrderAsync());
            _commandCloseAndCreateNew = ReactiveCommand.CreateAsyncTask(_ => _controller.CloseOrderAsync());
            _commandNewOrder = ReactiveCommand.Create();
            _commandActiveOrders = ReactiveCommand.Create();

            _commandStart = ReactiveCommand.CreateAsyncTask(
                this.WhenAnyValue(vm => vm.ProgressState).Select(state => state == OrderProgressState.NotStarted), 
                _ => _controller.StartOrderAsync());
            _commandDone = ReactiveCommand.CreateAsyncTask(
                this.WhenAnyValue(vm => vm.ProgressState).Select(state => state == OrderProgressState.InProgress), 
                _ => _controller.DoneOrderAsync());

            // Subscribe to commands
            _commandCloseOrder.SubscribeToObserveOnUi(_ => NavigateToActiveOrders());
            _commandCloseAndCreateNew.SubscribeToObserveOnUi(_ => NavigateToNewOrder());
            _commandNewOrder.SubscribeToObserveOnUi(_ => NavigateToNewOrder());
            _commandActiveOrders.SubscribeToObserveOnUi(_ => NavigateToActiveOrders());
            _commandStart.Subscribe();
            _commandDone.Subscribe();

            _commandCloseOrder.ThrownExceptions
                .Merge(_commandCloseAndCreateNew.ThrownExceptions)
                .Merge(_commandStart.ThrownExceptions)
                .Merge(_commandDone.ThrownExceptions)
                .Subscribe(HandleError);

            _controller.OrderStatusStream
                .Select(status => status == OrderStatus.Prepared || status == OrderStatus.Closed
                    ? OrderProgressState.Prepared
                    : status == OrderStatus.InProgress
                        ? OrderProgressState.InProgress 
                        : OrderProgressState.NotStarted)
                .ToPropertyEx(this, vm => vm.ProgressState);

            _controller.ReceiptsStream.Take(1).Select(receipts => receipts.Sum(receipt => receipt.Total))
                .SubscribeToObserveOnUi(total => OrderAmount = total);
        }

        #endregion

        protected override void HandleError(Exception exception)
        {
            if (!(exception is ServerRuntimeException)) throw exception;
            Logger.Error(exception.Message, exception);
            DialogService.ShowErrorMessage(Resources.Dialog_Error_ServerRuntimeError);
        }

        #region Private methods

        private void NavigateToActiveOrders()
        {
            NavigationService.StartIntent(new Intent(AppNavigation.ActiveOrders).SetFlags(IntentFlags.ClearTop));
        }

        private void NavigateToNewOrder()
        {
            NavigationService.StartIntent(new Intent(AppNavigation.OrderMaker).SetFlags(IntentFlags.ClearTop | IntentFlags.NoHistory));
        }

        #endregion
    }
}
