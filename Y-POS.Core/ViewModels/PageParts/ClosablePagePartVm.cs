using System;
using System.Threading;

namespace Y_POS.Core.ViewModels.PageParts
{
    public abstract class ClosablePagePartVm : PosBaseVm
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
