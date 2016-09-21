using System;
using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.PageParts
{
    public interface IOrderMakerSetCustomerVm : ILifecycleVm
    {
        #region Properties

        string FirstName { get; set; }
        string LastName { get; set; }
        string Phone { get; set; }
        string Email { get; set; }

        string SearchText { get; set; }
        ICustomerItemVm[] Customers { get; }
        ICustomerItemVm SelectedCustomer { get; set; }

        #endregion

        #region Commands

        ICommand CommandCancel { get; }
        ICommand CommandOk { get; }

        #endregion

        #region Events

        event EventHandler CloseEvent;

        #endregion
    }
}