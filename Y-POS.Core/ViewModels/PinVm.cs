using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using Splat;
using YumaPos.Client.Common;
using YumaPos.Client.UI.ViewModels.Impl;

namespace Y_POS.Core.ViewModels
{
    public sealed class PinVm : PageVm, IPinVm
    {
        #region Fields

        private ReactiveCommand<Unit> _commandLogin;

        #endregion

        #region Properties

        public string UrlPathSegment { get { return "pin"; }}
        public IScreen HostScreen { get; private set; }

        #endregion

        #region Commands

        public ICommand CommandLogin { get { return _commandLogin; }}

        #endregion

        #region Constructor

        public PinVm(IScreen hostScreen)
        {
            HostScreen = hostScreen;
        }

        #endregion

        #region Lifecycle

        protected override void InitCommands()
        {
            _commandLogin = ReactiveCommand.CreateAsyncTask((o, token) => Task.Delay(1000, token));
        }

        protected override void InitLifetimeSubscriptions()
        {
            _commandLogin.ThrownExceptions.Subscribe(ex => this.Log().ErrorException(ex.Message, ex));
            _commandLogin.Subscribe(_ => HostScreen.Router.NavigateBack.Execute(null));
        }

        protected override void OnCreate(IArgsBundle args)
        {
            base.OnCreate(args);
        }

        #endregion
    }
}
