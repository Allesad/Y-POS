using System.Linq;
using YumaPos.Client.Common;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.Core.MenuModels;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.Items.Impl
{
    public class MenuCategoryItemVm : BaseVm, IMenuCategoryItemVm
    {
        #region Properties

        public string Uuid { get; }

        public IImageModel ImageModel { get; }

        public string Title { get; }

        #endregion

        #region Constructor

        public MenuCategoryItemVm(IMenuCategory model)
        {
            Uuid = model.CategoryId.ToString();
            Title = model.Languages.First(languageDto => languageDto.LanguageCode == "en").Name;
            ImageModel = ImageService.GetImage(model.ImageId);
        }

        #endregion

        public bool Equals(IIdentifiable other)
        {
            return other != null && Equals(Uuid, other.Uuid);
        }
    }
}