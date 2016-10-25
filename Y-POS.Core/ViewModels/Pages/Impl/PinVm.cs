using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Account;
using YumaPos.Client.Helpers;
using YumaPos.Client.Navigation;
using YumaPos.Client.Services;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.API.Enums;
using YumaPos.Shared.Core.Utils.Formating;
using Y_POS.Core.Infrastructure.Notifications;
using Y_POS.Core.Properties;

namespace Y_POS.Core.ViewModels.Pages
{
    public sealed class PinVm : PageVm, IPinVm
    {
        #region Fields

        private readonly IAccountServiceManager _accountServiceManager;
        private readonly IEmployeeClockService _employeeClockService;

        private ReactiveCommand<bool> _commandLogin;
        private ReactiveCommand<bool> _commandClockIn;
        private ReactiveCommand<bool> _commandClockOut;
        private ReactiveCommand<bool> _commandBreak;

        #endregion

        #region Properties

        [Reactive]
        public string FakePin { get; set; }

        private IToastManager Toast => ServiceLocator.Resolve<IToastManager>();

        #endregion

        #region Commands

        public ICommand CommandLogin => _commandLogin;

        public ICommand CommandClockIn => _commandClockIn;

        public ICommand CommandClockOut => _commandClockOut;

        public ICommand CommandBreak => _commandBreak;

        #endregion

        #region Constructor

        public PinVm(IAccountServiceManager accountServiceManager, IEmployeeClockService employeeClockService)
        {
            if (accountServiceManager == null) throw new ArgumentNullException(nameof(accountServiceManager));
            if (employeeClockService == null) throw new ArgumentNullException(nameof(employeeClockService));

            _accountServiceManager = accountServiceManager;
            _employeeClockService = employeeClockService;
        }

        #endregion

        #region Lifecycle

        protected override void InitCommands()
        {
            var canExecute = this.WhenAnyValue(vm => vm.FakePin)
                .Select(pin => !string.IsNullOrEmpty(pin) && pin.Length == 4);

            _commandLogin = ReactiveCommand.CreateAsyncTask(canExecute, pinFunc => _accountServiceManager.LoginByPin(ExtractPin(pinFunc)));
            _commandClockIn = ReactiveCommand.CreateAsyncTask(canExecute, pinFunc => ClockInAsync(ExtractPin(pinFunc)));
            _commandClockOut = ReactiveCommand.CreateAsyncTask(canExecute, pinFunc => ClockOutAsync(ExtractPin(pinFunc)));
            _commandBreak = ReactiveCommand.CreateAsyncTask(canExecute, pinFunc => BreakAsync(ExtractPin(pinFunc)));
        }

        protected override void InitLifetimeSubscriptions()
        {
            _commandLogin.Subscribe(_ => NavigationService.StartIntent(new Intent(AppNavigation.ActiveOrders).SetFlags(IntentFlags.NewHistory)));
            _commandClockIn.Subscribe();
            _commandClockOut.Subscribe();
            _commandBreak.Subscribe();
            
            _commandLogin.ThrownExceptions
                .Merge(_commandClockIn.ThrownExceptions)
                .Merge(_commandClockOut.ThrownExceptions)
                .Merge(_commandBreak.ThrownExceptions)
                .Subscribe(HandleError);
        }

        #endregion

        #region Private methods

        private static string ExtractPin(object pinFunc)
        {
            return ((Func<string>) pinFunc)();
        }

        private async Task<bool> ClockInAsync(string pin)
        {
            await _accountServiceManager.LoginByPin(pin);
            var state = await _employeeClockService.GetEmployeeClockState();
            if (state.IsClockIn)
            {
                ShowActionAlreadyPerformedNotification();
                return false;
            }
            await PerformOperation(UserActivityType.ClockIn);
            return true;
        }

        private async Task<bool> ClockOutAsync(string pin)
        {
            await _accountServiceManager.LoginByPin(pin);
            var state = await _employeeClockService.GetEmployeeClockState();
            if (!state.IsClockIn)
            {
                ShowActionAlreadyPerformedNotification();
                return false;
            }
            await PerformOperation(UserActivityType.ClockOut);
            return true;
        }

        private async Task<bool> BreakAsync(string pin)
        {
            await _accountServiceManager.LoginByPin(pin);
            var state = await _employeeClockService.GetEmployeeClockState();
            await PerformOperation(state.IsBreakStart ? UserActivityType.BreakEnd : UserActivityType.BreakStart);
            return true;
        }

        private async Task<bool> PerformOperation(UserActivityType operation)
        {
            switch (operation)
            {
                case UserActivityType.ClockIn:
                    await _employeeClockService.ClockInAsync();
                    break;
                case UserActivityType.ClockOut:
                    await _employeeClockService.ClockOutAsync();
                    break;
                case UserActivityType.BreakStart:
                    await _employeeClockService.BreakStartAsync();
                    break;
                case UserActivityType.BreakEnd:
                    await _employeeClockService.BreakEndAsync();
                    break;
            }
            ShowOperationCompletedNotification(operation);
            return true;
        }

        private void ShowActionAlreadyPerformedNotification()
        {
            /*var activityName = string.Empty;
            switch (activityType)
            {
                case UserActivityType.ClockIn:
                    break;
                case UserActivityType.ClockOut:
                    break;
                case UserActivityType.BreakStart:
                    break;
                case UserActivityType.BreakEnd:
                    break;
            }*/
            Toast.Show("Operation already performed");
            //return DialogService.ShowNotificationMessageAsync("Operation already performed");
        }

        private void ShowOperationCompletedNotification(UserActivityType activityType)
        {
            var username = FormattingUtils.FullName(_accountServiceManager.User.FirstName,
                _accountServiceManager.User.LastName);
            var message = string.Empty;
            switch (activityType)
            {
                case UserActivityType.ClockIn:
                    message = Resources.Toast_UserClockedIn;
                    break;
                case UserActivityType.ClockOut:
                    message = Resources.Toast_UserClockedOut;
                    break;
                case UserActivityType.BreakStart:
                    message = Resources.Toast_UserBreakStarted;
                    break;
                case UserActivityType.BreakEnd:
                    message = Resources.Toast_UserBreakEnded;
                    break;
            }

            Toast.Show(string.Format(message, username));
            //return DialogService.ShowNotificationMessageAsync(string.Format(message, username));
        }

        #endregion
    }
}
