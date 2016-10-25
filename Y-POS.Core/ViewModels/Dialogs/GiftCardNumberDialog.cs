using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using DialogManagement.Contracts;
using DialogManagement.Core;

namespace Y_POS.Core.ViewModels.Dialogs
{
    public sealed class GiftCardNumberDialog : PosBaseVm, IDialogButtonsConfigProvider, IDisposable
    {
        #region Fields

        private readonly IDisposable _msrSubscription;

        #endregion

        #region Properties

        public string CardNumber { get; private set; }

        #endregion

        #region Constructor

        public GiftCardNumberDialog()
        {
            _msrSubscription = Msr.GetDataStream()
                .Where(data => data.SuccessfulRead)
                .Subscribe(data => CardNumber = data.AccountNumber);
        }

        #endregion

        public IEnumerable<DialogButtonConfig> GetButtons()
        {
            return DefaultButtonSets.OkCancel;
        }

        public void Dispose()
        {
            _msrSubscription.Dispose();
        }
    }
}
