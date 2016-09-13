using System.Linq;
using YumaPos.Client.Common;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.Core.MenuModels;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.Items.Impl
{
    public class MenuItemItemVm : BaseVm, IMenuItemItemVm
    {
        #region Properties

        public string Uuid { get; }

        public IImageModel ImageModel { get; }

        public string Title { get; }

        public decimal Price { get; }

        #endregion

        #region Constructor

        public MenuItemItemVm(IMenuItem model)
        {
            Uuid = model.MenuItemId.ToString();
            Title = model.Language.First(languageDto => languageDto.LanguageCode == "en").Name;
            Price = model.Price;
            ImageModel = ImageService.GetImage(model.Image);
        }

        #endregion

        public bool Equals(IIdentifiable other)
        {
            return other != null && Equals(Uuid, other.Uuid);
        }
    }
}