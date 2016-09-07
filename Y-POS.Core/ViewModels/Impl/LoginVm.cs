using System;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using YumaPos.Client.Common;
using YumaPos.Client.Navigation;
using YumaPos.Client.UI.ViewModels.Impl;

namespace Y_POS.Core.ViewModels
{
    public sealed class LoginVm : PageVm, ILoginVm
    {
        #region Fields

        private ReactiveCommand<Unit> _commandLogin; 

        #endregion

        #region Properties

        public string UrlPathSegment { get { return "login"; } }

        #endregion

        #region Commands

        public ICommand CommandLogin { get { return _commandLogin; }}

        #endregion

        #region Constructor

        public LoginVm()
        {
            
        }

        #endregion

        protected override void InitCommands()
        {
            _commandLogin = ReactiveCommand.CreateAsyncTask((o, token) => Task.Delay(0, token));
        }

        protected override void InitLifetimeSubscriptions()
        {
            _commandLogin.ThrownExceptions.Subscribe(ex => Debug.WriteLine(ex.Message));
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
