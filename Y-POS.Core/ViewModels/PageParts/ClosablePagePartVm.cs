using System;
using System.Threading;
using YumaPos.Client.UI.ViewModels.Impl;

namespace Y_POS.Core.ViewModels.PageParts
{
    public abstract class ClosablePagePartVm : BaseVm
    {
        #region Events

        public event EventHandler CloseEvent;

        #endregion

        #region Methods

        protected void RaiseCloseEvent()
        {
            var handler = Volatile.Read(ref CloseEvent);
            handler?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
