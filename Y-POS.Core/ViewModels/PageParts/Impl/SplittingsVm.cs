using System;
using System.Windows.Input;
using ReactiveUI;
using Y_POS.Core.Checkout;

namespace Y_POS.Core.ViewModels.PageParts
{
    public sealed class SplittingsVm : ClosablePagePartVm, ISplittingsVm
    {
        #region Fields

        //private readonly CheckoutVmController _controller;
        private readonly CheckoutController _controller;
        //private readonly ICheckoutManager _checkoutManager;

        #endregion

        #region Commands

        public ICommand CommandCancel { get; }
        public ICommand CommandAllOnOne { get; }
        public ICommand CommandSplitEvenly { get; }

        #endregion

        #region Constructor

        public SplittingsVm(CheckoutController controller)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            //if (checkoutManager == null) throw new ArgumentNullException(nameof(checkoutManager));

            //_controller = controller;
            _controller = controller;
            //_checkoutManager = checkoutManager;

            var cmdCancel = ReactiveCommand.Create();
            cmdCancel.Subscribe(_ => RaiseCloseEvent());

            CommandCancel = cmdCancel;
            
            //var cmdAllOnOne = ReactiveCommand.CreateAsyncObservable(_ => _checkoutManager.SplitAllOnOne());
            var cmdAllOnOne = ReactiveCommand.CreateAsyncTask((_, ct) => _controller.SplitAllOnOneAsync(ct));
            cmdAllOnOne.Subscribe(_ => RaiseCloseEvent());

            CommandAllOnOne = cmdAllOnOne;

            /*var cmdSplitEvenly =
                ReactiveCommand.CreateAsyncObservable(param => _checkoutManager.SplitEvenly(int.Parse(param.ToString())));*/
            var cmdSplitEvenly =
                ReactiveCommand.CreateAsyncTask((param, ct) => _controller.SplitEvenlyAsync(Convert.ToInt32(param.ToString()), ct));
            cmdSplitEvenly.Subscribe(_ => RaiseCloseEvent());

            CommandSplitEvenly = cmdSplitEvenly;
        }

        #endregion
    }
}
