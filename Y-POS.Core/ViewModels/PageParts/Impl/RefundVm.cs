using System;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Y_POS.Core.Checkout;
using Y_POS.Core.Extensions;

namespace Y_POS.Core.ViewModels.PageParts
{
    public sealed class RefundVm : ClosablePagePartVm
    {
        #region Fields

        private readonly CheckoutController _controller;
        private readonly SelectedReceiptController _selectedReceiptController;

        #endregion

        #region Properties
        
        public extern decimal RefundAmount { [ObservableAsProperty] get; }

        #endregion

        #region Commands

        public ICommand CommandCancel { get; }
        public ICommand CommandRefund { get; }

        #endregion

        #region Constructor

        public RefundVm(CheckoutController controller, SelectedReceiptController selectedReceiptController)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            if (selectedReceiptController == null) throw new ArgumentNullException(nameof(selectedReceiptController));

            _controller = controller;
            _selectedReceiptController = selectedReceiptController;

            _selectedReceiptController.SelectedReceiptStream
                .Select(item => item?.TotalPaid ?? 0)
                .ToPropertyEx(this, vm => vm.RefundAmount, 0, SchedulerService.UiScheduler);

            // Cancel
            var cmdCancel = ReactiveCommand.Create();
            cmdCancel.Subscribe(_ => RaiseCloseEvent());

            CommandCancel = cmdCancel;

            // Refund
            var cmdRefund = ReactiveCommand.CreateAsyncTask((_, ct) => _controller.RefundAsync(_selectedReceiptController.CurrentReceipt));
            cmdRefund.Subscribe(response =>
            {
                if (!response.IsSuccess)
                {
                    DialogService.ShowErrorMessage(response.Message);
                    return;
                }
                RaiseCloseEvent();
            });

            CommandRefund = cmdRefund;
        }

        #endregion
    }
}
