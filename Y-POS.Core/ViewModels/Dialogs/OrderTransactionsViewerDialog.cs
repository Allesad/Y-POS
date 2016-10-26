using System;
using System.Collections.Generic;
using DialogManagement.Contracts;
using DialogManagement.Core;
using Y_POS.Core.ViewModels.Items.Impl;

namespace Y_POS.Core.ViewModels.Dialogs
{
    public sealed class OrderTransactionsViewerDialog : PosBaseVm, IDialogButtonsConfigProvider
    {
        #region Properties

        public IEnumerable<OrderTransactionItemVm> Transactions { get; }

        #endregion

        #region Constructor

        public OrderTransactionsViewerDialog(IEnumerable<OrderTransactionItemVm> transactions)
        {
            if (transactions == null) throw new ArgumentNullException(nameof(transactions));

            Transactions = transactions;
        }

        #endregion

        #region IDialogButtonsConfigProvider

        public IEnumerable<DialogButtonConfig> GetButtons()
        {
            return DefaultButtonSets.Ok;
        }

        #endregion
    }
}
