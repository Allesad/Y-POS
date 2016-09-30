﻿using System;
using System.Windows.Input;
using ReactiveUI;
using Y_POS.Core.Checkout;

namespace Y_POS.Core.ViewModels.PageParts
{
    public sealed class DiscountVm : ClosablePagePartVm, IDiscountVm
    {
        #region Fields

        private readonly CheckoutVmController _controller;

        #endregion

        #region Commands

        public ICommand CommandCancel { get; }

        #endregion

        #region Constructor

        public DiscountVm(CheckoutVmController controller)
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
