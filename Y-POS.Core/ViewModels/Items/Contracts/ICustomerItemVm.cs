using System;
using YumaPos.Client.Common;
using YumaPos.Client.UI.ViewModels.Contracts;
using YumaPos.Shared.API.Enums;

namespace Y_POS.Core.ViewModels.Items.Contracts
{
    public interface ICustomerItemVm : IBaseVm, IIdentifiable, IImageContainable
    {
        #region Properties

        string FirstName { get; }
        string LastName { get; }
        string FullName { get; }
        string Phone { get; }
        string Email { get; }
        string CardNumber { get; }
        DateTime? BirthDate { get; }
        string Comments { get; }
        Gender? Sex { get; }

        #endregion
    }
}