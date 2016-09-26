using System;
using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;
using YumaPos.Shared.API.Enums;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.PageParts
{
    public interface ISelectCustomerVm : ILifecycleVm
    {
        #region Properties
        
        bool IsNewCustomer { get; }
        string SearchText { get; set; }
        ICustomerItemVm[] Customers { get; }
        ICustomerItemVm SelectedCustomer { get; set; }
        bool IsCustomerDetailsVisible { get; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime? BirthDate { get; set; }
        Gender? Sex { get; set; }
        string Phone { get; set; }
        string Email { get; set; }

        #endregion

        #region Commands

        ICommand CommandGoToFindCustomer { get; }
        ICommand CommandGoToNewCustomer { get; }
        ICommand CommandCancel { get; }
        ICommand CommandSubmit { get; }

        #endregion

        #region Events

        event EventHandler CancelEvent;

        #endregion
    }
}