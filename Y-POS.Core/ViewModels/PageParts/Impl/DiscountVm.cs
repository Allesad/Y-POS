using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Shared.API.Models;
using Y_POS.Core.Checkout;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Items.Impl;

namespace Y_POS.Core.ViewModels.PageParts
{
    public sealed class DiscountVm : ClosablePagePartVm, IDiscountVm
    {
        #region Fields

        private readonly CheckoutController _controller;
        private readonly SelectedReceiptController _selectedReceiptController;

        #endregion

        #region Properties

        [Reactive]
        public DiscountItemVm[] Discounts { get; private set; }

        #endregion

        #region Commands

        public ICommand CommandCancel { get; }
        public ICommand CommandSetDiscount { get; }

        #endregion

        #region Constructor

        public DiscountVm(CheckoutController controller, SelectedReceiptController selectedReceiptController)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            if (selectedReceiptController == null) throw new ArgumentNullException(nameof(selectedReceiptController));

            _controller = controller;
            _selectedReceiptController = selectedReceiptController;

            var cmdCancel = ReactiveCommand.Create();
            cmdCancel.Subscribe(_ => RaiseCloseEvent());

            CommandCancel = cmdCancel;

            var cmdSetDiscount = ReactiveCommand.CreateAsyncTask((param, ct) => _controller.ApplyDiscountAsync(_selectedReceiptController.CurrentReceipt, (DiscountDto) param, ct));
            cmdSetDiscount.Subscribe(_ => RaiseCloseEvent());

            CommandSetDiscount = cmdSetDiscount;

            _controller.GetDiscountsAsync()
                .Select(dtos => dtos.Select(dto => new DiscountItemVm(dto)).ToArray())
                .SubscribeToObserveOnUi(vms => Discounts = vms);
        }

        #endregion
    }
}
