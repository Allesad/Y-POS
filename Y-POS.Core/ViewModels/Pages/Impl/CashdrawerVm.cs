using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Services;
using YumaPos.Shared.API.Models;
using Y_POS.Core.Cashdrawer;
using Y_POS.Core.Extensions;
using Y_POS.Core.Properties;
using Y_POS.Core.ViewModels.Items.Impl;

namespace Y_POS.Core.ViewModels.Pages
{
    public enum CashierBy
    {
        Amount,
        BillType
    }

    public enum CashdrawerState
    {
        CashierIn,
        CashierOut,
        Check,
        BankWithdraw,
        AddTips,
        CashIn,
        CashOut,
        PerformanceLog
    }

    public sealed class CashdrawerVm : PosPageVm, ICashdrawerVm
    {
        #region Fields
        
        private readonly CashierManager _cashier;
        private readonly ICashDrawerService _cashDrawerService;

        private ReactiveCommand<bool> _commandCashierIn;
        private ReactiveCommand<bool> _commandCashierOut;
        private ReactiveCommand<bool> _commandPerformOperation;
        private ReactiveCommand<object> _commandOpenDrawer;
        private ReactiveCommand<object> _commandGoToState;
        private ReactiveCommand<CashierLogItemVm[]> _commandUpdateLog;

        #endregion

        #region Properties

        [Reactive]
        public CashdrawerState State { get; private set; }
        [Reactive]
        public CashierBy CashierBy { get; set; }

        public extern bool IsAmountInputEnabled { [ObservableAsProperty] get;}
        public extern bool IsBillTypeInputEnabled { [ObservableAsProperty] get; }
        
        [Reactive]
        public BillTypeItem[] BillTypes { get; private set; }

        [Reactive]
        public CashierLogItemVm[] LogItems { get; private set; }
        [Reactive]
        public CashDrawerSummary Summary { get; private set; }
        [Reactive]
        public decimal CheckTotal { get; set; }
        [Reactive]
        public bool IsCashierIn { get; private set; }

        #endregion

        #region Commands

        public ICommand CommandCashierIn => _commandCashierIn;
        public ICommand CommandCashierOut => _commandCashierOut;
        public ICommand CommandOpenDrawer => _commandOpenDrawer;
        public ICommand CommandGoToState => _commandGoToState;
        public ICommand CommandPerformOperation => _commandPerformOperation;
        public ICommand CommandUpdateLog => _commandUpdateLog;

        #endregion

        #region Constructor

        public CashdrawerVm(CashierManager cashier, ICashDrawerService cashDrawerService)
        {
            if (cashier == null) throw new ArgumentNullException(nameof(cashier));
            if (cashDrawerService == null) throw new ArgumentNullException(nameof(cashDrawerService));

            _cashier = cashier;
            _cashDrawerService = cashDrawerService;
        }

        #endregion

        #region Lifecycle

        protected override void InitCommands()
        {
            var cashierInStream = this.WhenAnyValue(vm => vm.IsCashierIn);
            _commandCashierIn = ReactiveCommand.CreateAsyncTask(cashierInStream.Select(isCashierIn => !isCashierIn), 
                (param, ct) => CashierInAsync((decimal) param));
            _commandCashierOut = ReactiveCommand.CreateAsyncTask(cashierInStream.Select(isCashierIn => isCashierIn), 
                (param, ct) => CashierOutAsync((decimal) param));
            _commandPerformOperation = ReactiveCommand.CreateAsyncTask(cashierInStream.Select(isCashierIn => isCashierIn), 
                (param, ct) => PerformOperationAsync((decimal) ((object[]) param)[0], (string) ((object[]) param)[1]));
            _commandUpdateLog = ReactiveCommand.CreateAsyncTask(_ => GetLogAsync());
            _commandOpenDrawer = ReactiveCommand.Create(cashierInStream.Select(isCashierIn => isCashierIn));
            _commandGoToState = ReactiveCommand.Create();
        }

        protected override void InitLifetimeSubscriptions()
        {
            // Track cashier in state
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm._cashier.IsCashierIn)
                .SubscribeToObserveOnUi(isCashierIn => IsCashierIn = isCashierIn));

            // Track summary info
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm._cashier.Summary)
                .SubscribeToObserveOnUi(summary => Summary = summary));

            // Command Cashier In
            AddLifetimeSubscription(_commandCashierIn.Where(success => success).SubscribeToObserveOnUi(_ =>
            {
                State = CashdrawerState.PerformanceLog;
            }));
            
            // Command Cashier Out
            AddLifetimeSubscription(_commandCashierOut.Where(success => success).SubscribeToObserveOnUi(_ =>
            {
                State = CashdrawerState.CashierIn;
            }));

            // Command Perform operation
            AddLifetimeSubscription(_commandPerformOperation.Where(success => success).SubscribeToObserveOnUi(_ =>
            {
                State = CashdrawerState.PerformanceLog;
            }));

            // Track log items
            AddLifetimeSubscription(_commandUpdateLog.SubscribeToObserveOnUi(logItems => LogItems = logItems));

            // Command Open drawer
            AddLifetimeSubscription(_commandOpenDrawer.SubscribeToObserveOnUi(_ => Toast.Show("Drawer opened")));

            // Command Go to state
            AddLifetimeSubscription(_commandGoToState.Select(param => (CashdrawerState) param).SubscribeToObserveOnUi(state => State = state));

            // Handle command exceptions
            AddLifetimeSubscription(_commandCashierIn.ThrownExceptions
                .Merge(_commandCashierOut.ThrownExceptions)
                .Merge(_commandPerformOperation.ThrownExceptions)
                .Merge(_commandUpdateLog.ThrownExceptions)
                .Subscribe(HandleError));

            // CashierBy tracking
            var cashierByStream = this.WhenAnyValue(vm => vm.CashierBy);
            AddLifetimeSubscription(cashierByStream.Select(cashierBy => cashierBy == CashierBy.Amount)
                .ToPropertyEx(this, vm => vm.IsAmountInputEnabled));
            AddLifetimeSubscription(cashierByStream.Select(cashierBy => cashierBy == CashierBy.BillType)
                .ToPropertyEx(this, vm => vm.IsBillTypeInputEnabled));
        }

        protected override async void OnStart()
        {
            if (!_cashier.IsInitialized)
            {
                await _cashier.InitAsync();
                State = _cashier.IsCashierIn ? CashdrawerState.PerformanceLog : CashdrawerState.CashierIn;
            }
            else
            {
                State = _cashier.IsCashierIn ? CashdrawerState.PerformanceLog : CashdrawerState.CashierIn;
            }

            _cashDrawerService.GetMoneyTypes()
                .Select(types => types.Select(type => new BillTypeItem((int) type.Value))
                    .Concat(new[]
                    {
                        new BillTypeItem(), 
                    }).ToArray())
                .SubscribeToObserveOnUi(items => BillTypes = items, HandleError);
        }

        #endregion

        #region Private methods

        private async Task<bool> CashierInAsync(decimal amount)
        {
            if (_cashier.Summary.Balance != amount && !await AllowOverrideBalance(amount))
            {
                return false;
            }
            await _cashier.CashierInAsync(amount);
            return true;
        }

        private async Task<bool> CashierOutAsync(decimal amount)
        {
            if (_cashier.Summary.Balance != amount && !await AllowOverrideBalance(amount))
            {
                return false;
            }
            await _cashier.CashierOutAsync(amount);
            return true;
        }

        private async Task<bool> PerformOperationAsync(decimal amount, string reason)
        {
            if (amount < 0)
            {
                await DialogService.ShowErrorMessageAsync("Amount cannot be lower than 0");
                return false;
            }
            if (State != CashdrawerState.Check && amount == 0)
            {
                await DialogService.ShowErrorMessageAsync("Amount should be greater than 0");
                return false;
            }

            switch (State)
            {
                case CashdrawerState.Check:
                    await _cashier.PerformCheckAsync(amount);
                    break;
                case CashdrawerState.BankWithdraw:
                    await _cashier.BankWithdrawAsync(amount);
                    break;
                case CashdrawerState.AddTips:
                    await _cashier.AddTipsAsync(amount, reason);
                    break;
                case CashdrawerState.CashIn:
                    await _cashier.AddCashAsync(amount, reason);
                    break;
                case CashdrawerState.CashOut:
                    if (amount > _cashier.Summary.Balance)
                    {
                        await DialogService.ShowErrorMessageAsync(Resources.Dialog_Error_Cashdrawer_AmountGtreaterBalance);
                        return false;
                    }
                    await _cashier.ExtractCashAsync(amount, reason);
                    break;
                default:
                    throw new InvalidOperationException("Invalid state! " + State);
            }
            return true;
        }

        private Task<CashierLogItemVm[]> GetLogAsync()
        {
            return _cashDrawerService.GetLogResponse(GetFilter())
                .Select(dto => dto.Results)
                .Select(dtos => dtos.Select(dto => new CashierLogItemVm(dto)).ToArray()).ToTask();
        }

        private Task<bool> AllowOverrideBalance(decimal providedAmount)
        {
            return
                DialogService.CreateConfirmationDialog(
                    string.Format(Resources.Dialog_Confirmation_CheckCashdrawerAmountMismatch, providedAmount.ToString("C"),
                        _cashier.Summary.Balance.ToString("C"))).ShowAsync();
        }

        private CashDrawerItemsFilterDto GetFilter()
        {
            return new CashDrawerItemsFilterDto
            {
                Offset = 0,
                Count = 30,
                Activity = null,
                DateStart = DateTime.Today.AddMonths(-6),
                DateEnd = DateTime.Now,
                Sort = new FilteredRequestSortDto
                {
                    Desc = true,
                    Selector = "Date"
                }
            };
        }

        #endregion
    }
}
