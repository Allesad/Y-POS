using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Services;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.API.Models;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Items.Contracts;
using Y_POS.Core.ViewModels.Items.Impl;

namespace Y_POS.Core.ViewModels
{
    public sealed class SelectCustomerVm : BaseVm
    {
        #region Fields

        private readonly ICustomersService _customersService;

        private readonly ReactiveCommand<object> _commandGoToFindCustomer;
        private readonly ReactiveCommand<object> _commandGoToNewCustomer;

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

        public CustomerDto NewCustomer { get; private set; }

        #endregion

        #region Commands

        public ICommand CommandGoToFindCustomer => _commandGoToFindCustomer;
        public ICommand CommandGoToNewCustomer => _commandGoToNewCustomer;
        public ICommand CommandCancel { get; set; }
        public ICommand CommandSubmit { get; set; }

        #endregion

        #region Constructor

        public SelectCustomerVm(ICustomersService customersService, CustomerDto preselectedCustomer = null)
        {
            if (customersService == null) throw new ArgumentNullException(nameof(customersService));

            _customersService = customersService;

            _commandGoToFindCustomer = ReactiveCommand.Create(this.WhenAnyValue(vm => vm.IsNewCustomer).Select(b => b));
            _commandGoToNewCustomer = ReactiveCommand.Create(this.WhenAnyValue(vm => vm.IsNewCustomer).Select(b => !b));

            _commandGoToFindCustomer.Subscribe(_ => IsNewCustomer = false);
            _commandGoToNewCustomer.Subscribe(_ => IsNewCustomer = true);

            this.WhenAnyValue(vm => vm.SelectedCustomer).Skip(1)
                .Select(vm => vm != null)
                .ToPropertyEx(this, vm => vm.IsCustomerDetailsVisible);

            if (preselectedCustomer != null)
            {
                SelectedCustomer = new CustomerItemVm(preselectedCustomer);
            }

            NewCustomer = new CustomerDto();
            
            // Search customers
            this.WhenAnyValue(vm => vm.SearchText).Skip(1)
                .Throttle(TimeSpan.FromMilliseconds(300), SchedulerService.UiScheduler)
                .Select(GetCustomers)
                .Switch()
                .Select(dto => dto.Results.Select(customerDto => new CustomerItemVm(customerDto)).ToArray())
                .SubscribeToObserveOnUi(vms => Customers = vms);
        }

        #endregion

        #region Private methods

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

        #endregion
    }
}
