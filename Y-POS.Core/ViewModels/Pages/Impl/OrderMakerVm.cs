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
using Y_POS.Core.Properties;
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
        private readonly IGiftCardsVm _giftCardsVm;
        private readonly IOrderMakerSetCustomerVm _setCustomerVm;

        private ReactiveCommand<object> _commandAddCustomer;
        private ReactiveCommand<object> _commandDeleteItem;
        private ReactiveCommand<object> _commandModifyItem;
        private ReactiveCommand<Unit> _commandClear;
        private ReactiveCommand<bool> _commandVoid;
        private ReactiveCommand<object> _commandGiftCards;
        private ReactiveCommand<Unit> _commandPrint;
        private ReactiveCommand<object> _commandCheckout;

        #endregion

        #region Properties
        
        public extern int OrderNumber { [ObservableAsProperty] get; }

        public extern string CustomerName { [ObservableAsProperty] get; }

        [Reactive]
        public string SearchItemText { get; set; }
        [Reactive]
        public decimal Total { get; private set; }

        public IReactiveDerivedList<IOrderedItemVm> OrderedItems { get; private set; }
        [Reactive]
        public IOrderedItemVm SelectedItem { get; set; }

        [Reactive]
        public OrderMakerDetailsType DetailsType { get; private set; }

        [Reactive]
        public ILifecycleVm DetailsVm { get; private set; }

        #endregion

        #region Commands

        public ICommand CommandAddCustomer => _commandAddCustomer;
        public ICommand CommandDeleteItem => _commandDeleteItem;
        public ICommand CommandModifyItem => _commandModifyItem;
        public ICommand CommandClear => _commandClear;
        public ICommand CommandVoid => _commandVoid;
        public ICommand CommandGiftCards => _commandGiftCards;
        public ICommand CommandPrint => _commandPrint;
        public ICommand CommandCheckout => _commandCheckout;

        #endregion

        #region Constructor

        public OrderMakerVm(IOrderCreator orderCreator, IOrderMakerMenuVm menuVm, IOrderItemConstructorVm itemConstructorVm, 
            IGiftCardsVm giftCardsVm, IOrderMakerSetCustomerVm orderMakerSetCustomerVm)
        {
            if (orderCreator == null) throw new ArgumentNullException(nameof(orderCreator));
            if (menuVm == null) throw new ArgumentNullException(nameof(menuVm));
            if (itemConstructorVm == null) throw new ArgumentNullException(nameof(itemConstructorVm));
            if (giftCardsVm == null) throw new ArgumentNullException(nameof(giftCardsVm));
            if (orderMakerSetCustomerVm == null) throw new ArgumentNullException(nameof(orderMakerSetCustomerVm));

            _orderCreator = orderCreator;
            _menuVm = menuVm;
            _itemConstructorVm = itemConstructorVm;
            _giftCardsVm = giftCardsVm;
            _setCustomerVm = orderMakerSetCustomerVm;
        }

        #endregion

        #region Lifecycle

        protected override IEnumerable<ILifecycleVm> GetChildren()
        {
            return new ILifecycleVm[]{ _menuVm, _itemConstructorVm, _giftCardsVm, _setCustomerVm };
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
            var canExecuteItemOperation = this.WhenAnyValue(vm => vm.SelectedItem).Skip(1).Select(item => item != null);
            var canClear = this.WhenAnyValue(vm => vm.OrderedItems.Count).Select(count => count > 0);

            _commandAddCustomer = ReactiveCommand.Create();
            _commandDeleteItem = ReactiveCommand.Create(canExecuteItemOperation);
            _commandModifyItem = ReactiveCommand.Create(canExecuteItemOperation);
            _commandClear = ReactiveCommand.CreateAsyncObservable(canClear, _ => _orderCreator.ClearOrderItems());
            _commandVoid = ReactiveCommand.CreateAsyncTask(_ => VoidOrder(_orderCreator.OrderId));
            _commandGiftCards = ReactiveCommand.Create();
            _commandPrint = ReactiveCommand.CreateAsyncTask(_ => PrintOrder(_orderCreator.OrderId));
            _commandCheckout = ReactiveCommand.Create();
        }

        protected override void InitLifetimeSubscriptions()
        {
            // Order number
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm._orderCreator.Title)
                .Select(s => Convert.ToInt32(s))
                .ToPropertyEx(this, vm => vm.OrderNumber));

            // Customer name
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm._orderCreator.CustomerName)
                .Select(name => string.IsNullOrEmpty(name) ? Resources.AddCustomer : name)
                .ToPropertyEx(this, vm => vm.CustomerName));

            // Details type
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm.DetailsType)
                .Select(GetDetailsVmForType)
                .SubscribeToObserveOnUi(vm => DetailsVm = vm));

            // Delete item
            AddLifetimeSubscription(_commandDeleteItem.Select(param => (IOrderedItemVm) param)
                .SelectMany(item => _orderCreator.RemoveOrderItem(item.ToGuid()))
                .SubscribeToObserveOnUi());

            // Modify item
            AddLifetimeSubscription(_commandModifyItem.Select(param => (IOrderedItemVm) param)
                .SubscribeToObserveOnUi(orderedItem =>
                {
                    _itemConstructorVm.EditOrderItem(_orderCreator.OrderId, orderedItem);
                    DetailsType = OrderMakerDetailsType.ItemConstructor;
                }));

            // Clear items
            AddLifetimeSubscription(_commandClear.Subscribe());

            // Void
            AddLifetimeSubscription(_commandVoid.Where(b => b)
                .SelectMany(_ => _orderCreator.ChangeOrderStatus(OrderStatus.Void))
                .SubscribeToObserveOnUi(_ => NavigateTo(AppNavigation.ActiveOrders, IntentFlags.ClearTop)));

            // Add customer page
            AddLifetimeSubscription(_commandAddCustomer.Subscribe(_ => DetailsType = OrderMakerDetailsType.AddCustomer));

            // Gift cards page
            AddLifetimeSubscription(_commandGiftCards.Subscribe(_ => DetailsType = OrderMakerDetailsType.GiftCards));
            
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
            
            // Handle close details view
            AddLifetimeSubscription(Observable.FromEventPattern(
                    h => _itemConstructorVm.CloseEvent += h,
                    h => _itemConstructorVm.CloseEvent -= h)
                .Merge(Observable.FromEventPattern(
                    h => _giftCardsVm.CloseEvent += h,
                    h => _giftCardsVm.CloseEvent -= h))
                .Merge(Observable.FromEventPattern(
                    h => _setCustomerVm.CloseEvent += h,
                    h => _setCustomerVm.CloseEvent -= h))
                .Subscribe(_ => DetailsType = OrderMakerDetailsType.Menu));
        }

        #endregion

        #region Private methods

        private void NavigateTo(NavUri targetUri, IntentFlags flags = IntentFlags.None)
        {
            NavigationService.StartIntent(new Intent(targetUri).SetFlags(flags));
        }

        private ILifecycleVm GetDetailsVmForType(OrderMakerDetailsType type)
        {
            switch (type)
            {
                case OrderMakerDetailsType.ItemConstructor:
                    return _itemConstructorVm;
                case OrderMakerDetailsType.GiftCards:
                    return _giftCardsVm;
                case OrderMakerDetailsType.AddCustomer:
                    return _setCustomerVm;
                default:
                    return _menuVm;
            }
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
            if (args.HasModifiers)
            {
                DetailsType = OrderMakerDetailsType.ItemConstructor;
                _itemConstructorVm.ProcessMenuItem(args.MenuItem);
            }
            else
            {
                _orderCreator.AddOrderItem(args.MenuItem.ToGuid())
                    .SubscribeToObserveOnUi(
                        item =>
                            SelectedItem =
                                OrderedItems.First(
                                    orderedItem => orderedItem.Uuid.Equals(item.Uuid, StringComparison.OrdinalIgnoreCase)), HandleError);
            }
        }

        #endregion
    }
}
