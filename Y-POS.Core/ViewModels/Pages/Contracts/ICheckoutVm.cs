﻿using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels.Pages
{
    public interface ICheckoutVm : IPageVm
    {
        #region Properties

        IBaseVm OptionVm { get; }

        #endregion
    }
}