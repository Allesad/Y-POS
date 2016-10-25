using System;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Account;
using YumaPos.Client.Common;
using YumaPos.Client.Navigation;

namespace Y_POS.Core.ViewModels.Pages
{
    public sealed class LoginVm : PosPageVm, ILoginVm
    {
        #region Fields

        private readonly IAccountServiceManager _accountServiceManager;

        private ReactiveCommand<bool> _commandLogin; 

        #endregion

        #region Properties

        #endregion

        #region Commands

        [Reactive]
        public string Username { get; set; }
        public ICommand CommandLogin => _commandLogin;

        #endregion

        #region Constructor

        public LoginVm(IAccountServiceManager accountServiceManager)
        {
            if (accountServiceManager == null) throw new ArgumentNullException(nameof(accountServiceManager));

            _accountServiceManager = accountServiceManager;
        }

        #endregion

        protected override void InitCommands()
        {
            _commandLogin = ReactiveCommand.CreateAsyncTask(this.WhenAnyValue(vm => vm.Username)
                .Select(s => !string.IsNullOrEmpty(s)), o =>
            {
                object[] par = (object[]) o;
                string username = (string) par[0];
                string pass = (string) par[1];
                return _accountServiceManager.LoginAsync("demo", "demo");
            });
        }

        protected override void InitLifetimeSubscriptions()
        {
            _commandLogin.ThrownExceptions.Subscribe(HandleError);
            _commandLogin.Subscribe(_ =>
            {
                NavigationService.StartIntent(new Intent(AppNavigation.ActiveOrders));
            });
        }

        protected override void OnCreate(IArgsBundle args)
        {
            Username = "Lalala";
        }

        protected override void OnStart()
        {
            base.OnStart();
        }
    }
}
