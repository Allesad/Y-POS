using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Common;
using YumaPos.Client.Extensions;
using YumaPos.Client.Navigation;
using YumaPos.Client.Services;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.API.Enums;
using Y_POS.Core.ViewModels.Items.Contracts;
using Y_POS.Core.ViewModels.Items.Impl;

namespace Y_POS.Core.ViewModels.Pages
{
    public class ActiveOrdersVm : PageVm, IActiveOrdersVm
    {
        private readonly IOrderService _orderService;

        #region Fields

        private ReactiveCommand<object> _commandCreateOrder;
        private ReactiveCommand<object> _commandCheckout;
        private ReactiveCommand<object> _commandPrintOrder;
        private ReactiveCommand<object> _commandVoid;

        #endregion

        #region Properties
        
        [Reactive]
        public IActiveOrderItemVm[] Items { get; private set; }
        [Reactive]
        public IActiveOrderItemVm SelectedItem { get; set; }

        #endregion

        #region Commands

        public ICommand CommandCreateOrder => _commandCreateOrder;
        public ICommand CommandCheckout => _commandCheckout;
        public ICommand CommandPrintOrder => _commandPrintOrder;
        public ICommand CommandVoid => _commandVoid;

        #endregion

        #region Constructor

        public ActiveOrdersVm(IOrderService orderService)
        {
            if (orderService == null) throw new ArgumentNullException(nameof(orderService));

            _orderService = orderService;
        }

        #endregion

        #region Lifecycle

        protected override void OnCreate(IArgsBundle args)
        {
        }

        protected override void InitCommands()
        {
            var canExecute = this.WhenAnyValue(vm => vm.SelectedItem).Select(vm => vm != null);

            _commandCreateOrder = ReactiveCommand.Create();
            _commandCheckout = ReactiveCommand.Create(canExecute);
            _commandPrintOrder = ReactiveCommand.Create(canExecute);
            _commandVoid = ReactiveCommand.Create(canExecute);
        }

        protected override void InitLifetimeSubscriptions()
        {
            AddLifetimeSubscription(_commandCreateOrder.Subscribe(_ => NavigationService.StartIntent(new Intent(AppNavigation.OrderMaker))));
            AddLifetimeSubscription(_commandCheckout.Where(_ => SelectedItem != null).Subscribe(_ => NavigateTo(new Intent(AppNavigation.Checkout)
                .SetArgs(new ArgsBundle().Put("id", SelectedItem.ToGuid())))));
            AddLifetimeSubscription(_commandPrintOrder
                .Where(o => o != null)
                .Select(o => (IActiveOrderItemVm) o)
                .Subscribe(async order => await Printer.PrintOrderAsync(order.ToGuid())));

            // Void order
            AddLifetimeSubscription(_commandVoid
                .Where(o => o != null)
                .Select(o => (IActiveOrderItemVm) o)
                .Subscribe(order => VoidOrder(order.ToGuid())));
        }

        protected override void OnStart()
        {
            _orderService.GetActiveOrdersResponse()
                .Select(dto => dto.Results.Select(orderDto => new ActiveOrderItemVm(orderDto)).ToArray())
                .ObserveOn(SchedulerService.UiScheduler)
                .Subscribe(dtos => Items = dtos);
        }

        #endregion

        #region Private methods

        private void NavigateTo(Intent intent)
        {
            try
            {
                NavigationService.StartIntent(intent);
            }
            catch (Exception ex)
            {
                DialogService.CreateMessageDialog(ex.Message, "Error").Show();
            }
        }

        private async void VoidOrder(Guid orderId)
        {
            var dlg = DialogService.CreateConfirmationDialog(Properties.Resources.Dialog_Confirmation_VoidOrder, "Confirmation");
            var res = await dlg.ShowAsync();
            if (res)
            {
                _orderService.UpdateOrderStatus(orderId, (int) OrderStatus.Void)
                    .Subscribe(_ => {}, () => ((ActiveOrderItemVm) Items.First(vm => vm.ToGuid() == orderId)).UpdateStatus(OrderStatus.Void));
            }
        }

        #endregion
    }
}
