using System;
using YumaPos.Client.UI.ViewModels.Impl;
using YumaPos.Shared.API.Enums;
using YumaPos.Shared.API.Models;
using YumaPos.Shared.Core.Utils.Formating;

namespace Y_POS.Core.ViewModels.Items.Impl
{
    public class CashierLogItemVm : BaseVm
    {
        #region Properties

        public DateTime Time { get; }
        public CashDrawerActivity Activity { get; }
        public decimal Amount { get; }
        public string EmployeeName { get; }

        #endregion

        public CashierLogItemVm(CashDrawerItemDto dto)
        {
            Time = dto.Date;
            Activity = dto.Activity;
            Amount = dto.Amount;
            EmployeeName = dto.Employee != null
                ? FormattingUtils.FullName(dto.Employee.FirstName, dto.Employee.LastName) : String.Empty;
        }
    }
}
