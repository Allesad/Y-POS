using System;
using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;
using YumaPos.Shared.API.Enums;

namespace Y_POS.Core.ViewModels.Items.Contracts
{
    public interface IActiveOrderItemVm : IEntityVm
    {
        #region Properties

        int OrderNumber { get; }
        DateTime CreationTime { get; }
        OrderStatus Status { get; }
        string CustomerName { get; }
        decimal Amount { get; }

        #endregion

        #region Commands

        ICommand CommandOpen { get; }

        #endregion
    }
}