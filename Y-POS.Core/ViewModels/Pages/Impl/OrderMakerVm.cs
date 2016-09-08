using System;
using System.Windows.Input;
using ReactiveUI;
using YumaPos.Client.Common;
using YumaPos.Client.Navigation;
using YumaPos.Client.UI.ViewModels.Impl;

namespace Y_POS.Core.ViewModels.Pages
{
    public sealed class OrderMakerVm : PageVm, IOrderMakerVm
    {
        #region Fields

        private ReactiveCommand<object> _commandCheckout; 

        #endregion

        #region Commands

        public int OrderNumber { get; private set; }
        public ICommand CommandCheckout { get { return _commandCheckout; } }

        #endregion

        #region Constructor

        public OrderMakerVm()
        {
        }

        #endregion

        #region Lifecycle

        protected override void OnCreate(IArgsBundle args)
        {
            if (args != null)
            {
                OrderNumber = args.GetInt("id");
            }
        }

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
