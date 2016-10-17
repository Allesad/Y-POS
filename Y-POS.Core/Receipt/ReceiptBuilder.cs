using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using YumaPos.Client.Builders;
using YumaPos.Client.Factories;
using Y_POS.Core.Properties;

namespace Y_POS.Core.Receipt
{
    public sealed class ReceiptBuilder : IReceiptBuilder
    {
        #region Fields

        private readonly IReceiptTemplateFactory _templateFactory;

        private string _logoUrl;
        private string _restaurantName;
        private string _address;
        private string _transactionNumber;
        private string _orderNumber;
        private string _date;
        private string _utcDate;
        private string _time;
        private string _utcTime;
        private string _waiterName;
        private string _cashierName;
        private string _customerName;
        private string _subTotal;
        private string _tips;
        private string _total;
        private string _totalPaid;
        private string _change;
        private string _lastPhrase;
        private string _storeName;
        private string _terminalName;
        private readonly IList<Tax> _taxes;
        private readonly IDictionary<string, string> _discounts;
        private readonly IList<OrderedItem> _orderedItems;
        private readonly IList<PaymentType> _paymentTypes;

        #endregion

        #region Constructor

        public ReceiptBuilder(IReceiptTemplateFactory templateFactory)
        {
            if (templateFactory == null)
                throw new ArgumentNullException(nameof(templateFactory));

            _templateFactory = templateFactory;

            _taxes = new List<Tax>();
            _discounts = new Dictionary<string, string>();
            _orderedItems = new List<OrderedItem>();
            _paymentTypes = new List<PaymentType>();
        }

        #endregion

        #region IReceiptBuilder

        public IReceiptBuilder SetLogo(string logoUrl)
        {
            _logoUrl = logoUrl;
            return this;
        }

        public IReceiptBuilder SetRestaurantName(string restaurantName)
        {
            _restaurantName = restaurantName;
            return this;
        }

        public IReceiptBuilder SetAddress(string address)
        {
            _address = address;
            return this;
        }

        public IReceiptBuilder SetTransactionNumber(string transactionNumber)
        {
            _transactionNumber = transactionNumber;
            return this;
        }

        public IReceiptBuilder SetOrderNumber(string orderNumber)
        {
            _orderNumber = orderNumber;
            return this;
        }

        public IReceiptBuilder SetDate(string date)
        {
            _date = date;
            return this;
        }

        public IReceiptBuilder SetUtcDate(string utcDate)
        {
            _utcDate = utcDate;
            return this;
        }

        public IReceiptBuilder SetTime(string time)
        {
            _time = time;
            return this;
        }

        public IReceiptBuilder SetUtcTime(string utcTime)
        {
            _utcTime = utcTime;
            return this;
        }

        public IReceiptBuilder SetWaiterName(string waiterName)
        {
            _waiterName = waiterName;
            return this;
        }

        public IReceiptBuilder SetCashierName(string cashierName)
        {
            if (!string.IsNullOrEmpty(cashierName))
            {
                _cashierName = $"{Resources.Cashier}: {cashierName}";
            }
            return this;
        }

        public IReceiptBuilder SetCustomerName(string customerName)
        {
            _customerName = customerName;
            return this;
        }

        public IReceiptBuilder SetSubTotal(string subTotal)
        {
            _subTotal = subTotal;
            return this;
        }

        public IReceiptBuilder SetTips(string tips)
        {
            _tips = tips;
            return this;
        }

        public IReceiptBuilder SetTotal(string total)
        {
            _total = total;
            return this;
        }

        public IReceiptBuilder SetTotalPaid(string totalPaid)
        {
            _totalPaid = totalPaid;
            return this;
        }

        public IReceiptBuilder SetChange(string change)
        {
            _change = change;
            return this;
        }

        public IReceiptBuilder AddTax(string taxName, string taxRate, string taxPrice)
        {
            _taxes.Add(new Tax(taxName, taxRate, taxPrice));
            return this;
        }

        public IReceiptBuilder AddItem(string title, string modifiers, string qty, string pricePerOne,
            string priceForAll, string amount, string discount)
        {
            _orderedItems.Add(new OrderedItem(title, modifiers, qty, pricePerOne, priceForAll, amount, discount));
            return this;
        }

        public IReceiptBuilder SetLastPhrase(string lastPhrase)
        {
            _lastPhrase = lastPhrase;
            return this;
        }

        public IReceiptBuilder AddDiscount(string discountName, string discountValue)
        {
            _discounts.Add(discountName, discountValue);
            return this;
        }

        public IReceiptBuilder SetStoreName(string storeName)
        {
            _storeName = storeName;
            return this;
        }

        public IReceiptBuilder SetTerminalName(string terminalName)
        {
            _terminalName = terminalName;
            return this;
        }

        public IReceiptBuilder AddPaymentType(string name, string amount)
        {
            _paymentTypes.Add(new PaymentType
            {
                Amount = amount,
                Name = name
            });

            return this;
        }

        public string Build()
        {
            const string scripts =
                "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" /> " +
                @"<script>
                var down = 0                

                document.onmousemove = mousemove;
                document.onmousedown = function(e) {
                  e = e || window.event;
                  down = 1;
                  x = e.clientX;
                  y = e.clientY;
                }
 
                document.onmouseup = function(e){
                    e = e || window.event;
                    down = 0;
                }
 
                function mousemove(e) {
                 if(down == 1){
                  if (x && y) {
                    window.scrollBy(x - e.clientX, y - e.clientY);
                  }
                  x = e.clientX;
                  y = e.clientY;
                 }
                }
            </script>";

            string styles = _templateFactory.GetStyles();
            string template = _templateFactory.GetReceiptTemplate();

            var orderedItemsSb = new StringBuilder();

            foreach (var item in _orderedItems)
            {
                string itemTemplate = _templateFactory.GetOrderedItemTemplate();

                var orderedItemParams = new Dictionary<string, string>
                {
                    {"Title", item.Title},
                    {"Modifiers", item.Modifiers},
                    {"Qty", item.Qty},
                    {"PricePerOne", item.PricePerOne},
                    {"PriceForAll", item.PriceForAll},
                    {"ItemAmount", item.Amount},
                    {"ItemDiscount", item.Discount},
                };

                orderedItemsSb.Append(Regex.Replace(itemTemplate, @"\{(.+?)\}",
                    match => orderedItemParams[match.Groups[1].Value]));
            }

            var taxesSb = new StringBuilder();

            foreach (var tax in _taxes)
            {
                string taxTemplate = _templateFactory.GetTaxTemplate();

                var taxParams = new Dictionary<string, string>
                {
                    {"TaxName", tax.Title},
                    {"TaxValue", tax.Amount},
                    {"TaxRate", tax.Rate}
                };

                taxesSb.Append(Regex.Replace(taxTemplate, @"\{(.+?)\}", match => taxParams[match.Groups[1].Value]));
            }

            var discountsSb = new StringBuilder();
            foreach (var discount in _discounts)
            {
                string discTemplate = _templateFactory.GetDiscountTemplate();

                var discountParams = new Dictionary<string, string>
                {
                    {"DiscountName", discount.Key},
                    {"DiscountValue", discount.Value}
                };

                discountsSb.Append(Regex.Replace(discTemplate, @"\{(.+?)\}",
                    match => discountParams[match.Groups[1].Value]));
            }

            var paymentTypesSb = new StringBuilder();
            foreach (var type in _paymentTypes)
            {
                string typeTemplate = _templateFactory.GetPaymentTypeTemplate();

                var typesParams = new Dictionary<string, string>
                {
                    {"PaymentTypeName", type.Name},
                    {"PaymentTypeAmount", type.Amount}
                };

                paymentTypesSb.Append(Regex.Replace(typeTemplate, @"\{(.+?)\}",
                    match => typesParams[match.Groups[1].Value]));
            }

            var parameters = new Dictionary<string, string>
            {
                {"LogoUrl", _logoUrl},
                {"RestaurantName", _restaurantName},
                {"Address", _address},
                {"TransactionNumber", _transactionNumber},
                {"OrderNumber", _orderNumber},
                {"Date", _date},
                {"UtcDate", _utcDate},
                {"Time", _time},
                {"UtcTime", _utcTime},
                {"WaiterName", _waiterName},
                {"CashierName", _cashierName},
                {"CustomerName", _customerName},
                {"SubTotal", _subTotal},
                {"Tips", _tips},
                {"Total", _total},
                {"TotalPaid", _totalPaid},
                {"Change", _change},
                {"LastPhrase", _lastPhrase},
                {"StoreName", _storeName},
                {"TerminalName", _terminalName},
                {"OrderedItems", orderedItemsSb.ToString()},
                {"Taxes", taxesSb.ToString()},
                {"Discounts", discountsSb.ToString()},
                {"PaymentTypes", paymentTypesSb.ToString()}
            };

            template = Regex.Replace(template, @"\{(.+?)\}", match => parameters[match.Groups[1].Value]);

            Clean();

            return new StringBuilder()
                .Append(scripts)
                .Append(styles)
                .Append(template)
                .ToString();
        }

        public string BuildFromReceipt(YumaPos.Client.Builders.Receipt receipt)
        {
            if (receipt == null)
                throw new ArgumentNullException(nameof(receipt));

            Clean();

            SetLogo(receipt.StoreLogo)
                .SetRestaurantName(receipt.StoreName)
                .SetStoreName(receipt.RealStoreName)
                .SetAddress(receipt.StoreAddress)
                .SetTransactionNumber(receipt.TransactionNumber)
                .SetOrderNumber(receipt.OrderNumber)
                .SetDate(receipt.Date)
                .SetUtcDate(receipt.UtcDate)
                .SetTime(receipt.Time)
                .SetUtcTime(receipt.UtcTime)
                .SetWaiterName(receipt.WaiterName)
                .SetCashierName(receipt.CashierName)
                .SetCustomerName(receipt.CustomerName)
                .SetSubTotal(receipt.SubTotal)
                .SetTips(receipt.Tips)
                .SetTotal(receipt.TotalAmount)
                .SetTotalPaid(receipt.TotalPayed)
                .SetChange(receipt.Change)
                .SetLastPhrase(receipt.EndingPhrase)
                .SetTerminalName(receipt.TerminalName);

            if (receipt.PaymentTypes != null)
            {
                foreach (var type in receipt.PaymentTypes.ToArray())
                {
                    AddPaymentType(type.PaymentTypeName, type.Amount);
                }
            }

            if (receipt.Taxes != null)
            {
                foreach (var tax in receipt.Taxes.ToArray())
                {
                    AddTax(tax.Title, tax.Rate, tax.Amount);
                }
            }

            if (receipt.Discounts != null)
            {
                foreach (var discount in receipt.Discounts.ToArray())
                {
                    AddDiscount("Discount", discount);
                }
            }

            if (receipt.OrderedItems == null)
                return Build();

            foreach (var orderedItem in receipt.OrderedItems.ToArray())
            {
                var modifiers = $"{(orderedItem.RelatedModifiers.Any() ? string.Join(" / ", orderedItem.RelatedModifiers.Select(mod => $"{mod.Name} + {mod.Quantity} x {mod.Price}")) + " / " : string.Empty)} {(orderedItem.CommonModifiers.Any() ? string.Join(" / ", orderedItem.CommonModifiers.Select(mod => $"{mod.Name} + {mod.Quantity} x {mod.Price}")) + " / " : string.Empty)}";

                AddItem(orderedItem.Title, modifiers, orderedItem.Qty,
                    orderedItem.PricePerOne, orderedItem.PriceForAll, orderedItem.Amount, orderedItem.Discount);
            }

            return Build();
        }

        #endregion

        #region Private methods

        private void Clean()
        {
            _logoUrl = "";
            _restaurantName = "";
            _address = "";
            _transactionNumber = "";
            _orderNumber = "";
            _date = "";
            _utcDate = "";
            _time = "";
            _utcTime = "";
            _waiterName = "";
            _cashierName = "";
            _customerName = "";
            _subTotal = "";
            _total = "";
            _totalPaid = "";
            _change = "";
            _lastPhrase = "";
            _storeName = "";
            _terminalName = "";
            _taxes.Clear();
            _discounts.Clear();
            _orderedItems.Clear();
            _paymentTypes.Clear();
        }

        #endregion

        private class OrderedItem
        {
            public string Title { get; private set; }
            public string Modifiers { get; private set; }
            public string Qty { get; private set; }
            public string PricePerOne { get; private set; }
            public string PriceForAll { get; private set; }
            public string Amount { get; private set; }
            public string Discount { get; private set; }


            public OrderedItem(string title, string modifiers, string qty, string pricePerOne, string priceForAll,
                string amount, string discount)
            {
                Title = title;
                Modifiers = modifiers;
                Qty = qty;
                PricePerOne = pricePerOne;
                PriceForAll = priceForAll;
                Amount = amount;
                Discount = discount;
            }
        }

        private class Tax
        {
            public string Title { get; private set; }
            public string Rate { get; private set; }
            public string Amount { get; private set; }

            public Tax(string title, string rate, string amount)
            {
                Title = title;
                Rate = rate;
                Amount = amount;
            }
        }

        private class PaymentType
        {
            public string Name { get; set; }
            public string Amount { get; set; }
        }
    }
}