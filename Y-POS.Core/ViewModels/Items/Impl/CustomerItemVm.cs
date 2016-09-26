using System;
using YumaPos.Client.Common;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.API.Enums;
using YumaPos.Shared.API.Models;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.Items.Impl
{
    public sealed class CustomerItemVm : BaseVm, ICustomerItemVm
    {
        #region Properties

        public string Uuid { get; }
        public IImageModel ImageModel { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string FullName => $"{FirstName} {LastName}";
        public string Phone { get; }
        public string Email { get; }
        public string CardNumber { get; }
        public DateTime? BirthDate { get; }
        public string Comments { get; }
        public Gender? Sex { get; }

        #endregion

        #region Constructor

        public CustomerItemVm(CustomerDto model)
        {
            Uuid = model.CustomerId.ToString();
            FirstName = model.FirstName;
            LastName = model.LastName;
            Phone = model.HomePhone;
            Email = model.Email ?? "-";
            CardNumber = model.CardNo ?? "-";
            ImageModel = ImageService.GetImage(model.ImageId);
            BirthDate = model.BirthDate;
            Comments = model.Memo ?? "-";
            Sex = model.Sex;
        }

        #endregion

        public bool Equals(IIdentifiable other)
        {
            return Uuid.Equals(other.Uuid);
        }
    }
}
