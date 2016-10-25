using System.Collections.Generic;
using System.Threading;
using DialogManagement.Contracts;
using DialogManagement.Core;

namespace Y_POS.Core.ViewModels.Dialogs
{
    public sealed class ProcessingDialog : PosBaseVm, IDialogButtonsConfigProvider
    {
        #region Fields

        #endregion

        #region Properties

        public CancellationToken Token { get; }

        #endregion

        #region Commands
        
        #endregion

        #region Constructor

        public ProcessingDialog(CancellationToken ct)
        {
            Token = ct;
        }

        #endregion
        
        public IEnumerable<DialogButtonConfig> GetButtons()
        {
            return DefaultButtonSets.Cancel;
        }
    }
}
