using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Extensions;
using YumaPos.Client.Helpers;
using YumaPos.Client.Services;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.API.Enums;
using YumaPos.Shared.API.Models;
using YumaPos.Shared.Infrastructure;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Items.Contracts;
using Y_POS.Core.ViewModels.Items.Impl;

namespace Y_POS.Core.ViewModels.PageParts
{
    public sealed class SelectCustomerVm : PosLifecycleVm, ISelectCustomerVm
    {
        #region Fields

        private readonly ICustomersService _customersService;

        private ReactiveCommand<object> _commandGoToFindCustomer;
        private ReactiveCommand<object> _commandGoToNewCustomer;
        private ReactiveCommand<object> _commandCancel;
        private ReactiveCommand<CustomerDto> _commandSubmit;

        #endregion

        #region Properties

        [Reactive]
        public bool IsNewCustomer { get; private set; }

        [Reactive]
        public string SearchText { get; set; }

        [Reactive]
        public ICustomerItemVm[] Customers { get; private set; }

        [Reactive]
        public ICustomerItemVm SelectedCustomer { get; set; }

        public extern bool IsCustomerDetailsVisible { [ObservableAsProperty] get; }

        [Reactive]
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Sex { get; set; }

        [Reactive]
        public string Phone { get; set; }

        public string Email { get; set; }

        #endregion

        #region Commands

        public ICommand CommandGoToFindCustomer => _commandGoToFindCustomer;
        public ICommand CommandGoToNewCustomer => _commandGoToNewCustomer;
        public ICommand CommandCancel => _commandCancel;
        public ICommand CommandSubmit => _commandSubmit;

        #endregion

        #region Events

        public event EventHandler CancelEvent;
        public event EventHandler<CustomerSelectedEventArgs> CustomerSelectedEvent;

        #endregion

        #region Constructor

        public SelectCustomerVm(ICustomersService customersService)
        {
            if (customersService == null) throw new ArgumentNullException(nameof(customersService));

            _customersService = customersService;

            this.WhenAnyValue(vm => vm.SelectedCustomer).Skip(1)
                .Select(vm => vm != null)
                .ToPropertyEx(this, vm => vm.IsCustomerDetailsVisible);
        }

        #endregion

        #region Lifecycle

        protected override void InitCommands()
        {
            _commandGoToFindCustomer = ReactiveCommand.Create(this.WhenAnyValue(vm => vm.IsNewCustomer).Select(b => b));
            _commandGoToNewCustomer = ReactiveCommand.Create(this.WhenAnyValue(vm => vm.IsNewCustomer).Select(b => !b));

            _commandCancel = ReactiveCommand.Create();
            _commandSubmit = ReactiveCommand.CreateAsyncTask(IsFormValidStream(), _ => GetCustomerToSubmit());
        }

        protected override void InitLifetimeSubscriptions()
        {
            // Switch to customers list
            AddLifetimeSubscription(_commandGoToFindCustomer.Subscribe(_ => IsNewCustomer = false));

            // Switch to new customer form
            AddLifetimeSubscription(_commandGoToNewCustomer.Subscribe(_ => IsNewCustomer = true));

            // Cancel command
            AddLifetimeSubscription(_commandCancel.Subscribe(_ => RaiseCancelEvent()));

            // Submit command
            AddLifetimeSubscription(_commandSubmit.Subscribe(customer =>
            {
                SelectedCustomer = null;
                RaiseCustomerSelectedEvent(customer);
            }));

            // Search customers
            this.WhenAnyValue(vm => vm.SearchText).Skip(1)
                .Throttle(TimeSpan.FromMilliseconds(300), SchedulerService.UiScheduler)
                .Select(GetCustomers)
                .Switch()
                .Select(dto => dto.Results.Select(customerDto => new CustomerItemVm(customerDto)).ToArray())
                .SubscribeToObserveOnUi(vms => Customers = vms, HandleError);
        }

        #endregion

        #region Private methods

        private IObservable<bool> IsFormValidStream()
        {
            return this.WhenAny(vm => vm.IsNewCustomer, vm => vm.SelectedCustomer, vm => vm.FirstName, vm => vm.Phone,
                (isNewCustomer, selectedCustomer, firstName, phone) =>
                    selectedCustomer.Value != null ||
                    (isNewCustomer.Value && !string.IsNullOrEmpty(firstName.Value) && !string.IsNullOrEmpty(phone.Value)));
        } 

        private IObservable<FilteredCustomersResponseDto> GetCustomers(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Observable.Return(new FilteredCustomersResponseDto
                {
                    Count = 0,
                    Results = new CustomerDto[0]
                });
            }
            return _customersService.GetCustomers(count: 50, search: query);
        }

        private async Task<CustomerDto> GetCustomerToSubmit()
        {
            if (!IsNewCustomer)
                return new CustomerDto
                {
                    CustomerId = SelectedCustomer.ToGuid(),
                    FirstName = SelectedCustomer.FirstName,
                    LastName = SelectedCustomer.LastName,
                    HomePhone = SelectedCustomer.Phone,
                    Email = SelectedCustomer.Email,
                    Sex = SelectedCustomer.Sex
                };
            var customer = new CustomerDto
            {
                FirstName = FirstName,
                LastName = LastName,
                HomePhone = Phone,
                Email = Email,
                BirthDate = BirthDate,
                Sex = Sex
            };
            var response = await _customersService.AddCustomer(customer).ToTask();
            customer.CustomerId = response.Value;
            return customer;
        }

        private void RaiseCancelEvent()
        {
            var handler = Volatile.Read(ref CancelEvent);
            handler?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseCustomerSelectedEvent(CustomerDto customer)
        {
            var handler = Volatile.Read(ref CustomerSelectedEvent);
            handler?.Invoke(this, new CustomerSelectedEventArgs(customer));
        }

        #endregion
    }
}