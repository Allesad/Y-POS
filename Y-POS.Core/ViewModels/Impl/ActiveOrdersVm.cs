using System;
using System.Windows.Input;
using ReactiveUI;
using YumaPos.Client.Navigation;
using YumaPos.Client.UI.ViewModels.Impl;

namespace Y_POS.Core.ViewModels
{
    public class ActiveOrdersVm : PageVm, IActiveOrdersVm
    {
        #region Fields

        private ReactiveCommand<object> _commandCreateOrder;

        #endregion

        #region Properties

        #endregion

        #region Commands

        public ICommand CommandCreateOrder { get { return _commandCreateOrder; } }

        #endregion

        #region Constructor

        public ActiveOrdersVm()
        {
        }

        #endregion

        #region Lifecycle

        protected override void InitCommands()
        {
            _commandCreateOrder = ReactiveCommand.Create();
        }

        protected override void InitLifetimeSubscriptions()
        {
            AddLifetimeSubscription(_commandCreateOrder.Subscribe(_ => NavigationService.StartIntent(new Intent(AppNavigation.OrderMaker))));
        }

        #endregion
    }
}
