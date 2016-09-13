using System;
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
using YumaPos.Client.Services;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.API.Enums;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Items.Contracts;
using Y_POS.Core.ViewModels.Items.Impl;

namespace Y_POS.Core.ViewModels.Pages
{
    public sealed class OrderMakerVm : PageVm, IOrderMakerVm
    {
        #region Fields

        private readonly IOrderCreator _orderCreator;
        private readonly IMenuService _menuService;

        private ReactiveCommand<object> _commandAddCustomer;
        private ReactiveCommand<Unit> _commandClear;
        private ReactiveCommand<bool> _commandVoid;
        private ReactiveCommand<Unit> _commandPrint;
        private ReactiveCommand<object> _commandCheckout;

        #endregion

        #region Properties

        [ObservableAsProperty]
        public int OrderNumber { get; }
        [Reactive]
        public string SearchItemText { get; set; }
        [Reactive]
        public decimal Total { get; private set; }

        [Reactive]
        public IMenuCategoryItemVm[] Categories { get; private set; }
        [Reactive]
        public IMenuItemItemVm[] CategoryItems { get; private set; }

        public IReactiveDerivedList<IOrderedItem> OrderedItems { get; private set; }

        [Reactive]
        public IMenuCategoryItemVm SelectedCategory { get; set; }

        [Reactive]
        public IOrderedItem SelectedItem { get; set; }

        #endregion

        #region Commands

        public ICommand CommandAddCustomer { get { return _commandAddCustomer; } }
        public ICommand CommandClear { get { return _commandClear; } }
        public ICommand CommandVoid { get { return _commandVoid; } }
        public ICommand CommandPrint { get { return _commandPrint; } }
        public ICommand CommandCheckout { get { return _commandCheckout; } }

        #endregion

        #region Constructor

        public OrderMakerVm(IOrderCreator orderCreator, IMenuService menuService)
        {
            if (orderCreator == null) throw new ArgumentNullException(nameof(orderCreator));
            if (menuService == null) throw new ArgumentNullException(nameof(menuService));

            _orderCreator = orderCreator;
            _menuService = menuService;
        }

        #endregion

        #region Lifecycle

        protected override void OnCreate(IArgsBundle args)
        {
            Guid orderId = args != null ? args.GetGuid("id") : Guid.Empty;

            OrderedItems = _orderCreator.OrderedItems.CreateDerivedCollection(item => item);

            _orderCreator.OrderedItems.Changed.Select(_ => _orderCreator.OrderedItems.Sum(item => item.TotalPrice))
                .Subscribe(total => Total = total);
                //.ToPropertyEx(this, vm => vm.Total, 0, SchedulerService.UiScheduler);

            //_orderCreator.OrderedItems.Changed.Select(_ => true)
            //    .Merge(_orderCreator.OrderedItems.CountChanged.Select(_ => true))
            //    .Merge(_orderCreator.OrderedItems.ItemChanged.Select(_ => true))
            //    .Select(_ => _orderCreator.OrderedItems.Sum(item => item.TotalPrice))
            //    .ToPropertyEx(this, vm => vm.Total);

            //this.WhenAnyObservable(vm => vm._orderCreator.OrderedItems.Changed).Select(_ => true)
            //    .Merge(this.WhenAnyObservable(vm => vm._orderCreator.OrderedItems.CountChanged).Select(_ => true))
            //    .Merge(this.WhenAnyObservable(vm => vm._orderCreator.OrderedItems.ItemChanged).Select(_ => true))
            //    .Throttle(TimeSpan.FromMilliseconds(50), SchedulerService.UiScheduler)
            //    .Select(_ => _orderCreator.OrderedItems.Sum(item => item.TotalPrice))
            //    .ToPropertyEx(this, vm => vm.Total);

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
            _commandAddCustomer = ReactiveCommand.Create();
            _commandClear = ReactiveCommand.CreateAsyncObservable(_ => _orderCreator.ClearOrderItems());
            _commandVoid = ReactiveCommand.CreateAsyncTask(_ => VoidOrder(_orderCreator.OrderId));
            _commandPrint = ReactiveCommand.CreateAsyncTask(_ => PrintOrder(_orderCreator.OrderId));
            _commandCheckout = ReactiveCommand.Create();
        }

        protected override void InitLifetimeSubscriptions()
        {
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

            // Category selection
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm.SelectedCategory)
                .Where(vm => vm != null)
                .SelectMany(vm => _menuService.GetMenuItemsCollectionForCategory(vm.ToGuid()))
                .Select(items => items.Select(item => new MenuItemItemVm(item)).ToArray())
                .SubscribeToObserveOnUi(vms => CategoryItems = vms));
        }

        protected override void OnStart()
        {
            this.WhenAnyValue(vm => vm._orderCreator.Title)
                .Select(s => Convert.ToInt32(s))
                .ToPropertyEx(this, vm => vm.OrderNumber);

            _menuService.GetMenuCategoriesCollection()
                .Select(categories => categories.Select(category => new MenuCategoryItemVm(category)).ToArray())
                .SubscribeToObserveOnUi(vms => Categories = vms);
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

        #endregion
    }
}
