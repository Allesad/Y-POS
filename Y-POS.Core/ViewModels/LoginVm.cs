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
        public IScreen HostScreen { get; private set; }

        #endregion

        #region Commands

        public ICommand CommandLogin { get { return _commandLogin; }}

        #endregion

        #region Constructor

        public LoginVm(IScreen hostScreen)
        {
            HostScreen = hostScreen;
        }

        #endregion

        protected override void InitCommands()
        {
            _commandLogin = ReactiveCommand.CreateAsyncTask((o, token) => Task.Delay(2000, token));
        }

        protected override void InitLifetimeSubscriptions()
        {
            _commandLogin.ThrownExceptions.Subscribe(ex => Debug.WriteLine(ex.Message));
            _commandLogin.Subscribe(_ =>
            {
                //NavigationService.StartIntent(new Intent(new NavUri("auth", "pin")));
                HostScreen.Router.NavigateAndReset.Execute(new PinVm(HostScreen));
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
