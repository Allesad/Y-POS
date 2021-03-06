﻿using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels.Pages
{
    public interface ILoginVm : IPageVm
    {
        #region Properties

        string Username { get; set; }

        #endregion

        #region Commands

        ICommand CommandLogin { get; }

        #endregion
    }
}