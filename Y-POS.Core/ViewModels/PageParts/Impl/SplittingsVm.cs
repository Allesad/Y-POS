using System;
using System.Windows.Input;
using ReactiveUI;
using YumaPos.Client.Module.Checkout.Contracts;
using Y_POS.Core.Checkout;

namespace Y_POS.Core.ViewModels.PageParts
{
    public sealed class SplittingsVm : ClosablePagePartVm, ISplittingsVm
    {
        #region Fields

        private readonly CheckoutVmController _controller;
        private readonly ICheckoutManager _checkoutManager;

        #endregion

        #region Commands

        public ICommand CommandCancel { get; }
        public ICommand CommandAllOnOne { get; }
        public ICommand CommandSplitEvenly { get; }

        #endregion

        #region Constructor

        public SplittingsVm(CheckoutVmController controller, ICheckoutManager checkoutManager)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            if (checkoutManager == null) throw new ArgumentNullException(nameof(checkoutManager));

            _controller = controller;
            _checkoutManager = checkoutManager;

            var cmdCancel = ReactiveCommand.Create();
            cmdCancel.Subscribe(_ => RaiseCloseEvent());

            CommandCancel = cmdCancel;
            
            var cmdAllOnOne = ReactiveCommand.CreateAsyncObservable(_ => _checkoutManager.SplitAllOnOne());
            cmdAllOnOne.Subscribe(_ => RaiseCloseEvent());

            CommandAllOnOne = cmdAllOnOne;

            var cmdSplitEvenly =
                ReactiveCommand.CreateAsyncObservable(param => _checkoutManager.SplitEvenly(int.Parse(param.ToString())));
            cmdSplitEvenly.Subscribe(_ => RaiseCloseEvent());

            CommandSplitEvenly = cmdSplitEvenly;
        }

        #endregion
    }
}
