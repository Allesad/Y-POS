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
using YumaPos.Client.Navigation;
using YumaPos.Client.Services;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.API.Enums;
using Y_POS.Core.Extensions;

namespace Y_POS.Core.ViewModels.Pages
{
    public sealed class OrderMakerVm : PageVm, IOrderMakerVm
    {
        #region Fields

        private readonly IOrderCreator _orderCreator;

        private ReactiveCommand<object> _commandAddCustomer;
        private ReactiveCommand<object> _commandClear;
        private ReactiveCommand<bool> _commandVoid;
        private ReactiveCommand<Unit> _commandPrint;
        private ReactiveCommand<object> _commandCheckout;
        private Guid _orderId;

        #endregion

        #region Properties

        [ObservableAsProperty]
        public int OrderNumber { get; }
        [Reactive]
        public string SearchItemText { get; set; }
        [Reactive]
        public decimal Total { get; private set; }

        public IReactiveDerivedList<IOrderedItem> OrderedItems { get; private set; }

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

        public OrderMakerVm(IOrderCreator orderCreator)
        {
            if (orderCreator == null) throw new ArgumentNullException(nameof(orderCreator));

            _orderCreator = orderCreator;
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
            _commandClear = ReactiveCommand.Create();
            _commandVoid = ReactiveCommand.CreateAsyncTask(_ => VoidOrder(_orderId));
            _commandPrint = ReactiveCommand.CreateAsyncTask(_ => PrintOrder(_orderId));
            _commandCheckout = ReactiveCommand.Create();
        }

        protected override void InitLifetimeSubscriptions()
        {
            // Void
            AddLifetimeSubscription(_commandVoid.Where(b => b)
                .SubscribeToObserveOnUi(_ => NavigateTo(AppNavigation.ActiveOrders, IntentFlags.ClearTop)));
            
            // Print
            AddLifetimeSubscription(_commandPrint.SubscribeToObserveOnUi());

            // Navigate to checkout
            AddLifetimeSubscription(_commandCheckout.SubscribeToObserveOnUi(_ => NavigateTo(AppNavigation.Checkout, IntentFlags.NoHistory)));
        }

        protected override void OnStart()
        {
            this.WhenAnyValue(vm => vm._orderCreator.Title)
                .Select(s => Convert.ToInt32(s))
                .ToPropertyEx(this, vm => vm.OrderNumber);
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
