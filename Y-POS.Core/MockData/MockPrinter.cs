using System;
using System.Threading;
using System.Threading.Tasks;
using DialogManagement.Contracts;
using YumaPos.Client.Hardware;
using YumaPos.Hardware.Contracts.Models;

namespace Y_POS.Core.MockData
{
    public class MockPrinter : IPrintService
    {
        #region Fields

        private readonly IDialogManager _dialogManager;

        #endregion

        #region Constructor

        public MockPrinter(IDialogManager dialogManager)
        {
            if (dialogManager == null) throw new ArgumentNullException(nameof(dialogManager));

            _dialogManager = dialogManager;
        }

        #endregion

        public Task PrintOrderAsync(Guid orderId)
        {
            return PrintOrderAsync(orderId, CancellationToken.None);
        }

        public Task PrintOrderAsync(Guid orderId, CancellationToken ct)
        {
            return _dialogManager.CreateMessageDialog("Order send to print", "Print action").ShowAsync();
        }

        public void PrintReceipt(YumaPos.Client.Builders.Receipt receipt)
        {
            _dialogManager.CreateMessageDialog("Receipt send to print", "Print action").Show();
        }

        public void PrinReport(ReportToPrint report)
        {
            _dialogManager.CreateMessageDialog("Report send to print", "Print action").Show();
        }
    }
}
