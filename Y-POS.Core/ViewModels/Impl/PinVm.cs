using System;
using System.Windows.Input;
using ReactiveUI;
using Splat;
using YumaPos.Client.Common;
using YumaPos.Client.Navigation;
using YumaPos.Client.UI.ViewModels.Impl;

namespace Y_POS.Core.ViewModels
{
    public sealed class PinVm : PageVm, IPinVm
    {
        #region Fields

        private ReactiveCommand<object> _commandLogin;
        private readonly ICommand _commandClockIn;
        private readonly ICommand _commandClockOut;
        private ReactiveCommand<object> _commandBreak;

        #endregion

        #region Properties

        public string UrlPathSegment { get { return "pin"; }}

        #endregion

        #region Commands

        public ICommand CommandLogin { get { return _commandLogin; }}

        public ICommand CommandClockIn
        {
            get { return _commandClockIn; }
        }

        public ICommand CommandClockOut
        {
            get { return _commandClockOut; }
        }

        public ICommand CommandBreak
        {
            get { return _commandBreak; }
        }

        #endregion

        #region Constructor

        public PinVm()
        {
        }

        #endregion

        #region Lifecycle

        protected override void InitCommands()
        {
            _commandLogin = ReactiveCommand.Create();
            _commandBreak = ReactiveCommand.Create();
        }

        protected override void InitLifetimeSubscriptions()
        {
            _commandLogin.ThrownExceptions.Subscribe(ex => this.Log().ErrorException(ex.Message, ex));
            _commandLogin.Subscribe(_ => NavigationService.StartIntent(new Intent(AppNavigation.ActiveOrders)));
            _commandBreak.Subscribe(_ => NavigationService.Back());
        }

        protected override void OnCreate(IArgsBundle args)
        {
            base.OnCreate(args);
        }

        #endregion
    }
}
