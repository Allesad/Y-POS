using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using DialogManagement.Contracts;
using DialogManagement.Core;
using YumaPos.Client.UI.ViewModels.Impl;

namespace Y_POS.Core.ViewModels.Dialogs
{
    public sealed class GiftCardNumberDialog : BaseVm, IDialogButtonsConfigProvider, IDisposable
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
                .Take(1)
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
