using System;
using YumaPos.Shared.API.Models;

namespace Y_POS.Core.Cashdrawer
{
    public sealed class CashDrawerSummary
    {
        public CashDrawerSummary()
        {
        }

        public CashDrawerSummary(CashDrawerInfoDto dto)
        {
            Balance = dto.Balance;
            CalculatedBalance = dto.CalculatedBalance;
            CashInTotal = dto.CashInTotal;
            CashOutTotal = dto.CashOutTotal;
            BankWithdrawTotal = dto.BankWithdrawTotal;
            TipsTotal = dto.TipsTotal;
            RefundTotal = dto.RefundTotal;
            Sales = dto.Sales;
            CashierIn = dto.CashierIn;
            CashierOut = dto.CashierOut;
            InitialDayAmount = dto.InitialDayAmount;
            LastCheckDate = dto.LastCheckDate;
        }

        public decimal CashInTotal { get; internal set; }
        public decimal CashOutTotal { get; internal set; }
        public decimal BankWithdrawTotal { get; internal set; }
        public decimal TipsTotal { get; internal set; }
        public decimal RefundTotal { get; internal set; }
        public decimal Sales { get; internal set; }
        public decimal Balance { get; internal set; }
        public decimal CalculatedBalance { get; internal set; }
        public DateTime? LastCheckDate { get; internal set; }
        public decimal CashierIn { get; internal set; }
        public decimal CashierOut { get; internal set; }
        public decimal InitialDayAmount { get; internal set; }
    }
}
