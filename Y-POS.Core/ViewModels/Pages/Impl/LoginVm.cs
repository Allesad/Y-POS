using System;
using System.Windows.Input;
using ReactiveUI;
using YumaPos.Client.Account;
using YumaPos.Client.Common;
using YumaPos.Client.Navigation;
using YumaPos.Client.UI.ViewModels.Impl;

namespace Y_POS.Core.ViewModels.Pages
{
    public sealed class LoginVm : PageVm, ILoginVm
    {
        #region Fields

        private readonly IAccountServiceManager _accountServiceManager;

        private ReactiveCommand<bool> _commandLogin; 

        #endregion

        #region Properties

        #endregion

        #region Commands

        public ICommand CommandLogin { get { return _commandLogin; }}

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
            _commandLogin = ReactiveCommand.CreateAsyncTask(o =>
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
            base.OnCreate(args);
        }

        protected override void OnStart()
        {
            base.OnStart();
        }
    }
}
