using System;
using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.PageParts
{
    public interface IGiftCardsVm : ILifecycleVm
    {
        #region Properties

        IGiftCardItemVm[] CardTypes { get; }
        IGiftCardItemVm SelectedCardType { get; set; }

        string CardNumber { get; set; }
        decimal RefillAmount { get; set; }
        decimal Balance { get; }

        bool IsIssueCardsVisible { get; }
        bool IsRefillVisible { get; }
        bool IsBalanceVisible { get; }

        #endregion

        #region Commands

        ICommand CommandGoToIssueCard { get; }
        ICommand CommandGoToRefill { get; }
        ICommand CommandGoToBalance { get; }
        ICommand CommandCancel { get; }
        ICommand CommandDone { get; }

        #endregion

        #region Events

        event EventHandler CloseEvent;

        #endregion
    }
}