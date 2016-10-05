using System;
using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Impl;
using Y_POS.Core.Checkout;

namespace Y_POS.Core.ViewModels.PageParts
{
    public sealed class PaymentCompleteVm : BaseVm, IPaymentCompleteVm
    {
        private readonly CheckoutController _controller;

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Commands

        public ICommand CommandCloseOrder { get; }
        public ICommand CommandCloseAndCreateNewOrder { get; }
        public ICommand CommandNewOrder { get; }
        public ICommand CommandActiveOrders { get; }

        #endregion

        #region Constructor

        public PaymentCompleteVm(CheckoutController controller)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));

            _controller = controller;
        }

        #endregion
        
    }
}
