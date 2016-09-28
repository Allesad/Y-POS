using YumaPos.Client.Factories;

namespace Y_POS.Core.Receipt
{
    public sealed class ReceiptTemplateFactory : IReceiptTemplateFactory
    {
        #region Constants

        private const string STYLES = @"
<style>
        @font-face {
            font-family: 'DroidSans';
            src:url('DroidSans.ttf');
            font-weight: normal;
            font-style: normal;
        }
        html{overflow:hidden;} 
        body{
            -ms-user-select: none;
            user-select: none;
            font-family: 'DroidSans';

        }
        .logo-company{}
        .logo-company img{
            max-width: 380px;
            max-height: 200px;
        }

        .text-center{
            text-align: center;
        }
        .name-company{
            font-weight: bold;
            font-size: 18px;
            padding-top: 15px;
            padding-bottom: 10px;
        }
        .address{
            font-size: 12px;
            padding-bottom: 15px;
        }
        .transaction-info{
            border-top: 1px dashed black;
            padding-bottom: 15px;
            padding-top: 20px;
            width:100%;
            display: table;
        }
        .order-number{
            text-align: left;
            font-size: 12px;
            display: table-cell;
            width: 50%;
        }
        .more-info-about-transaction{
            text-align: right;
            display: table-cell;
            width: 50%;
            font-size: 12px;
        }
        .row-item{
            padding-top: 5px; text-align: left;
        }
        .name-item{
            width: 80%;
            display: table-cell;
            font-size: 12px;
        }
        .row-item{ display: table;width: 100%;}
        .container-item{
            border-top: 1px dashed black;
            border-bottom: 1px dashed black;
            padding-top: 20px;
            padding-bottom: 20px;
            margin-bottom: 20px;
        }
        .price-item{width: 20%; display: table-cell; text-align: right; font-weight: bold;font-size: 12px;}
        .total-container{
            display: table-row;width: 100%;
        }
        .row-total{
            width: 100%;
            display: table;
        }
        .tax-row{ display: table-row;width: 100%;font-size: 12px;}
        .tax-title{display: table-cell; width: 75%;text-align: right;}
        .tax-price{
            display: table-cell; width: 25%; font-weight: bold;text-align: right;min-width: 70px;
        }
        .total-row{display: table-row;
            width: 100%;
            font-size: 12px;}
        .total-title{display: table-cell; width: 75%;font-weight: bold;text-align: right;padding-bottom: 15px;padding-top: 15px; margin-top: 15px; border-top: 1px dashed black;
            border-bottom: 1px dashed black;font-weight: bold;}
        .total-price{display: table-cell; width: 20%;text-align: right;padding-bottom: 15px;padding-top: 15px; margin-top: 15px; border-top: 1px dashed black;
            border-bottom: 1px dashed black;font-weight: bold;  min-width: 70px; }
       .margin-bottom{
           margin-bottom: 15px;
       }
        .margin-top{
           margin-top: 15px;
       }
        .total-cell{
            float: right;
        }
        .address-padding{
          margin-top: 20px;
            padding-top: 20px;
            border-top: 1px dashed black;
        }

    </style>";

        private const string RECEIPT_TEMPLATE = @"

<table width='100%'>
    <tbody>
    <tr>
        <td>

            <div class='main-container'>
                <div class='text-center logo-company'>
                    <img src='{LogoUrl}'>
                </div>
                <div class='text-center name-company'>
                        {RestaurantName}
                </div>
                <div class='text-center address'>
                     {Address}
                </div>
                <div class='transaction-info'>
                        <div class='order-number'>
                            Local Date: {Date} {Time}<br/>
                            UTC Date: {UtcDate} {UtcTime}<br/>
                            Store: {StoreName}<br/>
                            Register: {TerminalName}<br/>
                        </div>
                        <div class='more-info-about-transaction'>
                            Order # {OrderNumber}<br/>
                            Trans #: {TransactionNumber}<br/>
                            Waiter: {WaiterName}<br/>
                            {CashierName}<br/>
                        </div>
                </div>
                </td>
                </tr>
    <tr>
    <td>

    <div class='container-item'>
                {OrderedItems}
    </div>
    <div class='total-cell'>
                      <div class='total-container'>
                            <div class='row-total margin-bottom'>
                                {Discounts} 
                                {Taxes}  
                                <div class='tax-row'>
                                    <div class='tax-title'>
                                        Sub Total
                                    </div>
                                    <div class='tax-price'>
                                        {SubTotal}
                                    </div>
                                </div>                             
                            </div>
                            
                            <div class='row-total margin-top'>
                                {PaymentTypes} 
                                <div class='tax-row'>
                                    <div class='tax-title'>
                                        Tips
                                    </div>
                                    <div class='tax-price'>
                                        {Tips}
                                    </div>
                                </div>
                                <br/>
                                <div class='tax-row'>
                                    <div class='total-title'>
                                        TOTAL
                                    </div>
                                    <div class='total-price'>
                                        {Total}
                                    </div>
                                </div>
                                <br/>
                                <div class='tax-row'>
                                    <div class='tax-title'>
                                        Change
                                    </div>
                                    <div class='tax-price'>
                                        {Change}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


            </div>
        </td>

    </tr>
    <tr>
        <td>
                <div class='text-center address-padding'>
                    {LastPhrase}
                </div>
        </td>
    </tr>
    </tbody>
    </table>";



        private const string ORDERED_ITEM_TEMPLATE =

            @"  <div class='row-item'>
                    <div class='name-item'>
                        {Title}, {Modifiers} {Qty} x {PricePerOne} = {PriceForAll} {ItemDiscount}
                    </div>
                    <div class='price-item'>
                        {ItemAmount}
                    </div>
                </div>";


        private const string TAX_TEMPLATE = @"

            <div class='tax-row'>
                <div class='tax-title'>
                    {TaxName}, {TaxRate} %
                </div>
                <div class='tax-price'>
                    {TaxValue}
                </div>
            </div>
";

        private const string DISCOUNT_TEMPLATE = @"
            <div class='tax-row'>
                <div class='tax-title'>
                    {DiscountName}
                </div>
                <div class='tax-price'>
                    -{DiscountValue}
                </div>
            </div>
";

        private const string PAYMENT_TYPE_TEMPLATE = @"
            <div class='tax-row'>
                <div class='tax-title'>
                    {PaymentTypeName}
                </div>
                <div class='tax-price'>
                    {PaymentTypeAmount}
                </div>
            </div>
";

        #endregion

        #region IReceiptTemplateManager

        public string GetStyles()
        {
            return STYLES;
        }

        public string GetReceiptTemplate()
        {
            return RECEIPT_TEMPLATE;
        }

        public string GetOrderedItemTemplate()
        {
            return ORDERED_ITEM_TEMPLATE;
        }

        public string GetTaxTemplate()
        {
            return TAX_TEMPLATE;
        }

        public string GetDiscountTemplate()
        {
            return DISCOUNT_TEMPLATE;
        }

        public string GetPaymentTypeTemplate()
        {
            return PAYMENT_TYPE_TEMPLATE;
        }

        #endregion
    }
}
