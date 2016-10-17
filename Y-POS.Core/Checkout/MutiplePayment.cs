using System;
using System.Collections.Generic;
using System.Linq;
using YumaPos.Client.Services;
using YumaPos.Common.Infrastructure.BusinessLogic.Tendering;
using Y_POS.Core.Infrastructure;
using Y_POS.Core.ViewModels.Pages;

namespace Y_POS.Core.Checkout
{
    public sealed class MutiplePayment
    {
        #region Properties

        public Dictionary<PaymentType, decimal> PaymentParts { get; private set; }

        #endregion

        #region Constructor

        public MutiplePayment()
        {
            PaymentParts = new Dictionary<PaymentType, decimal>
            {
                { PaymentType.Cash, 0 },
                { PaymentType.Card, 0 },
                { PaymentType.GiftCard, 0 },
                { PaymentType.Mobile, 0 },
                { PaymentType.Points,  0 }
            };
        }

        #endregion

        #region Methods

        public void AddCash(decimal amount)
        {
            Guard.NotNegative(amount, nameof(amount));

            PaymentParts[PaymentType.Cash] = PaymentParts[PaymentType.Cash] + amount;
        }

        public void AddCard()
        {
            
        }

        public IList<TenderParams> ToTenderParams()
        {
            return PaymentParts.Select(pair => new TenderParams {Amount = pair.Value, TenderType = pair.Key.ToTenderType()}).ToList();
        } 

        #endregion
    }

    public static class TenderEx
    {
        public static TenderType ToTenderType(this PaymentType type)
        {
            switch (type)
            {
                case PaymentType.Cash:
                    return TenderType.Ca;
                case PaymentType.Card:
                    return TenderType.Cc;
                case PaymentType.GiftCard:
                    return TenderType.Eg;
                case PaymentType.Mobile:
                    return TenderType.Ch;
            }
            throw new InvalidOperationException($"Cannot convert {type} to TenderType");
        }


    }
}
