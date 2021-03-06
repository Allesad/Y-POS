﻿using System.Windows.Input;
using YumaPos.Client.UI.ViewModels.Contracts;

namespace Y_POS.Core.ViewModels.Pages
{
    public interface IPinVm : IPageVm
    {
        #region Properties

        #endregion

        #region Commands

        ICommand CommandLogin { get; }
        ICommand CommandClockIn { get; }
        ICommand CommandClockOut { get; }
        ICommand CommandBreak { get; }

        #endregion
    }
}