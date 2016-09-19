using System;
using System.Linq;
using YumaPos.Client.Common;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.API.Models;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.Items.Impl
{
    public sealed class GiftCardItemVm : BaseVm, IGiftCardItemVm
    {
        #region Properties

        public string Uuid { get; }
        public string Title { get; }
        public decimal Price { get; }

        #endregion

        #region Constructor

        public GiftCardItemVm(GiftCardTypeDto model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            Uuid = model.TypeId.ToString();
            Title = model.Languages.Any() ? model.Languages.First().Name : string.Empty;
            Price = model.Price;
        }

        #endregion

        public bool Equals(IIdentifiable other)
        {
            return Uuid.Equals(other.Uuid);
        }
    }
}