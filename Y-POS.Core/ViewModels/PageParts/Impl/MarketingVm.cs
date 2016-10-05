using System;
using System.Windows.Input;
using ReactiveUI;
using Y_POS.Core.Checkout;

namespace Y_POS.Core.ViewModels.PageParts
{
    public sealed class MarketingVm : ClosablePagePartVm, IMarketingVm
    {
        #region Fields

        private readonly CheckoutController _controller;

        #endregion

        #region Commands

        public ICommand CommandCancel { get; }

        #endregion

        #region Constructor

        public MarketingVm(CheckoutController controller)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            _controller = controller;

            var cmdCancel = ReactiveCommand.Create();
            cmdCancel.Subscribe(_ => RaiseCloseEvent());

            CommandCancel = cmdCancel;
        }

        #endregion
    }
}
