using YumaPos.Client.Common;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.API.Enums;
using YumaPos.Shared.API.Models;
using Y_POS.Core.Infrastructure;

namespace Y_POS.Core.ViewModels.Items.Impl
{
    public sealed class DiscountItemVm : BaseVm, IIdentifiable
    {
        #region Properties

        public string Uuid { get; }
        public string Name { get; }
        public DiscountType Type { get; }
        public string Description { get; }
        public decimal Value { get; }

        public DiscountDto Model { get; }

        #endregion

        #region Constructor

        public DiscountItemVm(DiscountDto model)
        {
            Guard.NotNull(model, nameof(model));

            Uuid = model.Id.ToString();
            Name = model.Name;
            Type = model.Type;
            Description = model.Description;
            Value = model.Value;

            Model = model;
        }

        #endregion

        public bool Equals(IIdentifiable other)
        {
            return other != null && Equals(Uuid, other.Uuid);
        }

    }
}
