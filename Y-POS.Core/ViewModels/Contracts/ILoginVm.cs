﻿using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels
{
    public interface ILoginVm : IPageVm
    {
        #region Properties

        #endregion

        #region Commands

        ICommand CommandLogin { get; }

        #endregion
    }
}