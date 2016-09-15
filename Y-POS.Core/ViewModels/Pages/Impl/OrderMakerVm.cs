using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Builders;
using YumaPos.Client.Common;
using YumaPos.Client.Extensions;
using YumaPos.Client.Navigation;
using YumaPos.Client.UI.ViewModels.Contracts;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.API.Enums;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Items.Contracts;
using Y_POS.Core.ViewModels.Items.Impl;
using Y_POS.Core.ViewModels.PageParts;

namespace Y_POS.Core.ViewModels.Pages
{
    public sealed class OrderMakerVm : PageVm, IOrderMakerVm
    {
        #region Fields

        private readonly IOrderCreator _orderCreator;

        private readonly IOrderMakerMenuVm _menuVm;
        private readonly IOrderItemConstructorVm _itemConstructorVm;

        private ReactiveCommand<object> _commandAddCustomer;
        private ReactiveCommand<object> _commandDeleteItem; 
        private ReactiveCommand<Unit> _commandClear;
        private ReactiveCommand<bool> _commandVoid;
        private ReactiveCommand<Unit> _commandPrint;
        private ReactiveCommand<object> _commandCheckout;

        private ReactiveCommand<object> _commandCancel;
        private ReactiveCommand<object> _commandDone;

        #endregion

        #region Properties
        
        [Reactive]
        public int OrderNumber { get; private set; }
        [Reactive]
        public string SearchItemText { get; set; }
        [Reactive]
        public decimal Total { get; private set; }
        
        public IReactiveDerivedList<IOrderedItemVm> OrderedItems { get; private set; }
        [Reactive]
        public IOrderedItemVm SelectedItem { get; set; }
        [Reactive]
        public ILifecycleVm DetailsVm { get; private set; }

        #endregion

        #region Commands

        public ICommand CommandAddCustomer => _commandAddCustomer;
        public ICommand CommandDeleteItem => _commandDeleteItem;
        public ICommand CommandClear => _commandClear;
        public ICommand CommandVoid => _commandVoid;
        public ICommand CommandPrint => _commandPrint;
        public ICommand CommandCheckout => _commandCheckout;
        public ICommand CommandCancelItemConstructor => _commandCancel;
        public ICommand CommandSubmitItemConstructor => _commandDone;

        #endregion

        #region Constructor

        public OrderMakerVm(IOrderCreator orderCreator, IOrderMakerMenuVm menuVm, IOrderItemConstructorVm itemConstructorVm)
        {
            if (orderCreator == null) throw new ArgumentNullException(nameof(orderCreator));
            if (menuVm == null) throw new ArgumentNullException(nameof(menuVm));
            if (itemConstructorVm == null) throw new ArgumentNullException(nameof(itemConstructorVm));

            _orderCreator = orderCreator;
            _menuVm = menuVm;
            _itemConstructorVm = itemConstructorVm;
        }

        #endregion

        #region Lifecycle

        protected override IEnumerable<ILifecycleVm> GetChildren()
        {
            return new ILifecycleVm[]{ _menuVm, _itemConstructorVm };
        }

        protected override void OnCreate(IArgsBundle args)
        {
            Guid orderId = args?.GetGuid("id") ?? Guid.Empty;

            DetailsVm = _menuVm;
            OrderedItems = _orderCreator.OrderedItems.CreateDerivedCollection(item => new OrderedItemVm(item));

            _orderCreator.OrderedItems.Changed.Select(_ => _orderCreator.OrderedItems.Sum(item => item.TotalPrice))
                .Subscribe(total => Total = total);
                //.ToPropertyEx(this, vm => vm.Total, 0, SchedulerService.UiScheduler);

            if (orderId != Guid.Empty)
            {
                _orderCreator.LoadOrder(orderId).SubscribeToObserveOnUi();
            }
            else
            {
                _orderCreator.Type = OrderType.Quick;
            }
        }

        protected override void InitCommands()
        {
            var canDeleteItem = this.WhenAnyValue(vm => vm.SelectedItem).Skip(1).Select(item => item != null);
            var canClear = this.WhenAnyValue(vm => vm.OrderedItems.Count).Select(count => count > 0);

            _commandAddCustomer = ReactiveCommand.Create();
            _commandDeleteItem = ReactiveCommand.Create(canDeleteItem);
            _commandClear = ReactiveCommand.CreateAsyncObservable(canClear, _ => _orderCreator.ClearOrderItems());
            _commandVoid = ReactiveCommand.CreateAsyncTask(_ => VoidOrder(_orderCreator.OrderId));
            _commandPrint = ReactiveCommand.CreateAsyncTask(_ => PrintOrder(_orderCreator.OrderId));
            _commandCheckout = ReactiveCommand.Create();

            _commandCancel = ReactiveCommand.Create();
            _commandDone = ReactiveCommand.Create();
        }

        protected override void InitLifetimeSubscriptions()
        {
            // Delete item
            AddLifetimeSubscription(_commandDeleteItem.Select(param => (IOrderedItemVm) param)
                .SelectMany(item => _orderCreator.RemoveOrderItem(item.ToGuid()))
                .SubscribeToObserveOnUi());

            // Clear items
            AddLifetimeSubscription(_commandClear.Subscribe(_ => DialogService.CreateMessageDialog("Order cleared").Show()));

            // Void
            AddLifetimeSubscription(_commandVoid.Where(b => b)
                .SelectMany(_ => _orderCreator.ChangeOrderStatus(OrderStatus.Void))
                .SubscribeToObserveOnUi(_ => NavigateTo(AppNavigation.ActiveOrders, IntentFlags.ClearTop)));
            
            // Print
            AddLifetimeSubscription(_commandPrint.SubscribeToObserveOnUi());

            // Navigate to checkout
            AddLifetimeSubscription(_commandCheckout.SubscribeToObserveOnUi(_ => NavigateTo(AppNavigation.Checkout, IntentFlags.NoHistory)));

            // Menu item selection
            AddLifetimeSubscription(Observable.FromEventPattern<MenuItemSelectedEventArgs>(
                    h => _menuVm.MenuItemSelected += h,
                    h => _menuVm.MenuItemSelected -= h)
                .Select(pattern => pattern.EventArgs)
                .SubscribeToObserveOnUi(OnMenuItemSelected));

            // Cancel order item constructor
            AddLifetimeSubscription(_commandCancel.SubscribeToObserveOnUi(_ =>
            {
                _itemConstructorVm.Cancel();
                DetailsVm = _menuVm;
            }));

            // Done orer item constructor
            AddLifetimeSubscription(_commandDone.SubscribeToObserveOnUi(_ =>
            {
                _itemConstructorVm.Cancel();
                DetailsVm = _menuVm;
            }));
        }

        protected override void OnStart()
        {
            this.WhenAnyValue(vm => vm._orderCreator.Title)
                .Select(s => Convert.ToInt32(s))
                .SubscribeToObserveOnUi(i => OrderNumber = i);
                //.ToPropertyEx(this, vm => vm.OrderNumber);
        }

        #endregion

        #region Private methods

        private void NavigateTo(NavUri targetUri, IntentFlags flags = IntentFlags.None)
        {
            NavigationService.StartIntent(new Intent(targetUri).SetFlags(flags));
        }

        private Task<bool> VoidOrder(Guid orderId)
        {
            return
                DialogService.CreateConfirmationDialog(Properties.Resources.Dialog_Confirmation_VoidOrder)
                    .ShowAsync();
        } 

        private Task PrintOrder(Guid orderId)
        {
            return Printer.PrintOrderAsync(orderId);
        }

        private void OnMenuItemSelected(MenuItemSelectedEventArgs args)
        {
            DetailsVm = _itemConstructorVm;
            _itemConstructorVm.ProcessMenuItem(args.MenuItem);
            //if (args.HasModifiers)
            //{
            //    DetailsVm = _itemConstructorVm;
            //    _itemConstructorVm.ProcessMenuItem(args.MenuItem);
            //}
            //else
            //{
            //    _orderCreator.AddOrderItem(args.MenuItem.ToGuid())
            //        .SubscribeToObserveOnUi(
            //            item =>
            //                SelectedItem =
            //                    OrderedItems.First(
            //                        orderedItem => orderedItem.Uuid.Equals(item.Uuid, StringComparison.OrdinalIgnoreCase)), HandleError);
            //}
        }

        #endregion
    }
}
