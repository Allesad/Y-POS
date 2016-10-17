using System.Linq;
using YumaPos.Client.App;
using YumaPos.Client.Helpers;
using YumaPos.Common.Infrastructure.BusinessLogic.Tendering;
using YumaPos.Shared.API.Models;
using YumaPos.Shared.Core.Utils.Formating;
using YumaPos.Shared.Core.Utils.Formating.Emums;
using Y_POS.Core.Properties;

namespace Y_POS.Core.Checkout
{
    public partial class ReceiptItem
    {
        private static YumaPos.Client.Builders.Receipt Map(RestaurantOrderReceiptDto dto)
        {
            var appService = ServiceLocator.Resolve<IAppService>();

            return new YumaPos.Client.Builders.Receipt
            {
                StoreLogo = appService.Store.Logo,
                StoreName = dto.SettingsStoreName,
                RealStoreName = appService.Store.Title,
                TerminalName = appService.Terminal.Name,
                StoreAddress = dto.SettingsStoreAddress,
                StorePhone = appService.Store.Phone,
                OrderNumber = dto.OrderNumber.ToString(),
                TransactionNumber = dto.TransactionNumber.ToString(),
                WaiterName = dto.WaiterName,
                CashierName = dto.CashierName,
                CustomerName = dto.CustomerName,
                Date = FormattingUtils.FormatDateTime(dto.Date.ToLocalTime(), TimeFormatType.None, DateFormatType.Short),
                UtcDate = FormattingUtils.FormatDateTime(dto.Date, TimeFormatType.None, DateFormatType.Short),
                Time = FormattingUtils.FormatShortTime(dto.Date.ToLocalTime()),
                UtcTime = FormattingUtils.FormatShortTime(dto.Date),
                SubTotal = FormattingUtils.FormatCurrency(dto.TotalAmount, CurrencyFormatType.RoundedNotSymboled),
                TotalAmount = FormattingUtils.FormatCurrency(dto.TotalAmount + dto.Tips, CurrencyFormatType.RoundedNotSymboled),
                TotalPayed = FormattingUtils.FormatCurrency(dto.TotalPaid, CurrencyFormatType.RoundedNotSymboled),
                Tips = FormattingUtils.FormatCurrency(dto.Tips, CurrencyFormatType.RoundedNotSymboled),
                Change = FormattingUtils.FormatCurrency(dto.Change, CurrencyFormatType.RoundedNotSymboled),
                Discounts = dto.Discount != null
                    ? new[] { FormattingUtils.FormatCurrency(dto.Discount.Amount, CurrencyFormatType.RoundedNotSymboled) }
                    : new string[] { },
                EndingPhrase = dto.SettingsEndingPhrase,
                Taxes = dto.Taxes.Select(taxDto => Map(taxDto)),
                OrderedItems = dto.OrderedItems.Select(itemDto => Map(itemDto)),
                PaymentTypes = dto.Tenders.Select(tender => new YumaPos.Client.Builders.Receipt.PaymentType
                {
                    Amount = FormattingUtils.FormatCurrency(tender.TenderAmount, CurrencyFormatType.RoundedNotSymboled),
                    PaymentTypeName = GetNameForTenderType(tender.Type)
                })
            };
        }

        private static YumaPos.Client.Builders.Receipt.OrderedItemTax Map(ReceiptTaxDto dto)
        {
            return new YumaPos.Client.Builders.Receipt.OrderedItemTax
            {
                Id = dto.TaxId,
                Amount = FormattingUtils.FormatCurrency(dto.TaxAmount, CurrencyFormatType.RoundedNotSymboled),
                Title = dto.TaxName,
                Rate = FormattingUtils.FormatNumber(dto.TaxRate, NumberFormatType.Percent)
            };
        }

        private static YumaPos.Client.Builders.Receipt.ReceiptOrderedItem Map(ReceiptOrderedItemDto dto)
        {
            return new YumaPos.Client.Builders.Receipt.ReceiptOrderedItem
            {
                Title = dto.Title,
                Qty = dto.Qty.ToString(),
                MenuItem = new YumaPos.Client.Builders.Receipt.ReceiptItem
                {
                    Name = dto.MenuItem.Name,
                    Price = FormattingUtils.FormatCurrency(dto.MenuItem.Price, CurrencyFormatType.RoundedNotSymboled),
                    Quantity = FormattingUtils.FormatNumber(dto.MenuItem.Quantity, NumberFormatType.WholeNumber)
                },
                RelatedModifiers = dto.RelatedModifiers.Select(mod => new YumaPos.Client.Builders.Receipt.ReceiptItem
                {
                    Name = mod.Name,
                    Price = FormattingUtils.FormatCurrency(mod.Price, CurrencyFormatType.RoundedNotSymboled),
                    Quantity = FormattingUtils.FormatNumber(mod.Quantity, NumberFormatType.WholeNumber)
                }),
                CommonModifiers = dto.CommonModifiers.Select(mod => new YumaPos.Client.Builders.Receipt.ReceiptItem
                {
                    Name = mod.Name,
                    Price = FormattingUtils.FormatCurrency(mod.Price, CurrencyFormatType.RoundedNotSymboled),
                    Quantity = FormattingUtils.FormatNumber(mod.Quantity, NumberFormatType.WholeNumber)
                }),
                PricePerOne = FormattingUtils.FormatCurrency(dto.Price, CurrencyFormatType.RoundedNotSymboled),
                PriceForAll = FormattingUtils.FormatCurrency(dto.Qty * dto.Price, CurrencyFormatType.RoundedNotSymboled),
                Amount =
                    FormattingUtils.FormatCurrency(
                        dto.Qty * dto.Price - (dto.ItemDiscount?.Amount ?? 0),
                        CurrencyFormatType.RoundedNotSymboled),
                Discount = dto.ItemDiscount != null
                    ? $" - {FormattingUtils.FormatCurrency(dto.ItemDiscount.Amount, CurrencyFormatType.RoundedNotSymboled)} {dto.ItemDiscount.Name}"
                    : string.Empty
            };
        }

        private static string GetNameForTenderType(TenderType type)
        {
            switch (type)
            {
                case TenderType.Ca:
                    return Resources.PaymentType_Cash;
                case TenderType.Eg:
                    return Resources.PaymentType_GiftCard;
                case TenderType.Cc:
                    return Resources.PaymentType_Card;
            }
            return string.Empty;
        }
    }
}
