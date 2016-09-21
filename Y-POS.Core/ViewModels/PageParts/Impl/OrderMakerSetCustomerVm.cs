using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Builders;
using YumaPos.Client.Extensions;
using YumaPos.Client.Services;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.API.Models;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Items.Contracts;
using Y_POS.Core.ViewModels.Items.Impl;

namespace Y_POS.Core.ViewModels.PageParts
{
    public sealed class OrderMakerSetCustomerVm : LifecycleVm, IOrderMakerSetCustomerVm
    {
        #region Fields

        private readonly IOrderCreator _orderCreator;
        private readonly ICustomersService _customersService;

        private ReactiveCommand<object> _commandCancel;
        private ReactiveCommand<Unit> _commandOk;

        #endregion

        #region Properties

        [Reactive]
        public string FirstName { get; set; }

        [Reactive]
        public string LastName { get; set; }

        [Reactive]
        public string Phone { get; set; }

        [Reactive]
        public string Email { get; set; }

        [Reactive]
        public string SearchText { get; set; }

        [Reactive]
        public ICustomerItemVm[] Customers { get; private set; }

        [Reactive]
        public ICustomerItemVm SelectedCustomer { get; set; }

        #endregion

        #region Commands

        public ICommand CommandCancel => _commandCancel;
        public ICommand CommandOk => _commandOk;

        #endregion

        #region Events

        public event EventHandler CloseEvent;

        #endregion

        #region Constructor

        public OrderMakerSetCustomerVm(ICustomersService customersService, IOrderCreator orderCreator)
        {
            if (customersService == null) throw new ArgumentNullException(nameof(customersService));
            if (orderCreator == null) throw new ArgumentNullException(nameof(orderCreator));

            _customersService = customersService;
            _orderCreator = orderCreator;
        }

        #endregion

        #region Lifecycle

        protected override void InitCommands()
        {
            var canExecuteOk = this.WhenAny(vm => vm.FirstName, vm => vm.Phone, vm => vm.SelectedCustomer,
                (firstName, phone, selectedCustomer) =>
                    (!string.IsNullOrEmpty(firstName.Value) && !string.IsNullOrEmpty(phone.Value))
                    || selectedCustomer.Value != null);

            _commandCancel = ReactiveCommand.Create();
            _commandOk = ReactiveCommand.CreateAsyncObservable(canExecuteOk, _ => GetCustomerOperation());
        }

        protected override void InitLifetimeSubscriptions()
        {
            // Cancel command
            AddLifetimeSubscription(_commandCancel.Subscribe(_ => RaiseCloseEvent()));

            // Ok command
            AddLifetimeSubscription(_commandOk.Subscribe(_ => RaiseCloseEvent()));

            // Search customers
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm.SearchText)
                .Where(s => !string.IsNullOrEmpty(s))
                .Throttle(TimeSpan.FromMilliseconds(300), SchedulerService.UiScheduler)
                .Select(query => _customersService.GetCustomers(count: 50, search: query))
                .Switch()
                .Select(dto => dto.Results.Select(customerDto => new CustomerItemVm(customerDto)).ToArray())
                .SubscribeToObserveOnUi(vms => Customers = vms));
        }

        private IObservable<Unit> GetCustomerOperation()
        {
            if (SelectedCustomer != null)
            {
                return
                    _orderCreator.SetCustomer(SelectedCustomer.ToGuid(), SelectedCustomer.FullName)
                        .Select(_ => Unit.Default);
            }
            return _customersService.AddCustomer(new CustomerDto
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    HomePhone = Phone,
                    Email = Email
                })
                .SelectMany(dto => _orderCreator.SetCustomer(dto.Value, $"{FirstName} {LastName}"))
                .Select(_ => Unit.Default);
        }

        private void RaiseCloseEvent()
        {
            var handler = Volatile.Read(ref CloseEvent);
            handler?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}