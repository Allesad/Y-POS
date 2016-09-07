using System;
using System.Windows.Input;
using ReactiveUI;
using YumaPos.Client.Navigation;
using YumaPos.Client.UI.ViewModels.Impl;

namespace Y_POS.Core.ViewModels
{
    public sealed class OrderMakerVm : PageVm, IOrderMakerVm
    {
        #region Fields

        private ReactiveCommand<object> _commandCheckout; 

        #endregion

        #region Commands

        public ICommand CommandCheckout { get { return _commandCheckout; } }

        #endregion

        #region Constructor

        public OrderMakerVm()
        {
        }

        #endregion

        #region Lifecycle

        protected override void InitCommands()
        {
            _commandCheckout = ReactiveCommand.Create();
        }

        protected override void InitLifetimeSubscriptions()
        {
            AddLifetimeSubscription(_commandCheckout.Subscribe(_ => NavigationService.StartIntent(new Intent(AppNavigation.Checkout))));
        }

        #endregion
    }
}
