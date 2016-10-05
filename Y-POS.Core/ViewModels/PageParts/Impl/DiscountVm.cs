using System;
using System.Windows.Input;
using ReactiveUI;
using Y_POS.Core.Checkout;

namespace Y_POS.Core.ViewModels.PageParts
{
    public sealed class DiscountVm : ClosablePagePartVm, IDiscountVm
    {
        #region Fields

        private readonly CheckoutController _controller;

        #endregion

        #region Commands

        public ICommand CommandCancel { get; }
        public ICommand CommandSetDiscount { get; }

        #endregion

        #region Constructor

        public DiscountVm(CheckoutController controller)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            _controller = controller;

            var cmdCancel = ReactiveCommand.Create();
            cmdCancel.Subscribe(_ => RaiseCloseEvent());

            CommandCancel = cmdCancel;

            var cmdSetDiscount = ReactiveCommand.CreateAsyncTask((param, ct) => _controller.SetDiscountAsync(param.ToString(), ct));
            cmdSetDiscount.Subscribe(_ => RaiseCloseEvent());

            CommandSetDiscount = cmdSetDiscount;
        }

        #endregion
    }
}
