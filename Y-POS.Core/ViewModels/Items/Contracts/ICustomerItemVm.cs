using YumaPos.Client.Common;
using YumaPos.Client.UI.ViewModels.Contracts;

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

        #endregion
    }
}