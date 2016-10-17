using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels.PageParts
{
    public interface ISplittingsVm : IBaseVm
    {
        #region Commands

        ICommand CommandCancel { get; }

        #endregion
    }
}