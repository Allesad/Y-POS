using System;
using System.Windows.Input;
using ReactiveUI;
using YumaPos.Client.Common;
using YumaPos.Client.Navigation;
using YumaPos.Client.Services;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.API.Enums;

namespace Y_POS.Core.ViewModels.Pages
{
    public sealed class OrderMakerVm : PageVm, IOrderMakerVm
    {
        #region Fields

        private readonly IOrderService _orderService;

        private ReactiveCommand<object> _commandCheckout;
        private Guid _orderId;

        #endregion

        #region Commands

        public int OrderNumber { get; private set; }
        public ICommand CommandCheckout { get { return _commandCheckout; } }

        #endregion

        #region Constructor

        public OrderMakerVm(IOrderService orderService)
        {
            if (orderService == null) throw new ArgumentNullException(nameof(orderService));

            _orderService = orderService;
        }

        #endregion

        #region Lifecycle

        protected override void OnCreate(IArgsBundle args)
        {
            if (args != null)
            {
                _orderId = args.GetGuid("id");

                _orderService.GetOrderById(_orderId).Subscribe(dto => OrderNumber = dto.Number);
            }
            else
            {
                _orderService.CreateNewOrder(OrderType.Quick).Subscribe(dto => OrderNumber = dto.Number);
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
