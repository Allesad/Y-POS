using System;
using System.Windows;
using System.Windows.Controls;

namespace Y_POS.Views.CashDrawerParts
{
    /// <summary>
    /// Interaction logic for CashDrawerSummaryView.xaml
    /// </summary>
    public partial class CashDrawerSummaryView : UserControl
    {
        public CashDrawerSummaryView()
        {
            InitializeComponent();

            CurrentDate.Text = DateTime.Now.ToShortDateString();
        }

        #region Balance

        public static readonly DependencyProperty BalanceProperty = DependencyProperty.Register(
            "Balance", typeof (decimal), typeof (CashDrawerSummaryView), new PropertyMetadata(default(decimal)));

        public decimal Balance
        {
            get { return (decimal) GetValue(BalanceProperty); }
            set { SetValue(BalanceProperty, value); }
        }

        #endregion

        #region Sales

        public static readonly DependencyProperty SalesProperty = DependencyProperty.Register(
            "Sales", typeof (decimal), typeof (CashDrawerSummaryView), new PropertyMetadata(default(decimal)));

        public decimal Sales
        {
            get { return (decimal) GetValue(SalesProperty); }
            set { SetValue(SalesProperty, value); }
        }

        #endregion

        #region Refund

        public static readonly DependencyProperty RefundProperty = DependencyProperty.Register(
            "Refund", typeof (decimal), typeof (CashDrawerSummaryView), new PropertyMetadata(default(decimal)));

        public decimal Refund
        {
            get { return (decimal) GetValue(RefundProperty); }
            set { SetValue(RefundProperty, value); }
        }

        #endregion

        #region Cash In

        public static readonly DependencyProperty CashInProperty = DependencyProperty.Register(
            "CashIn", typeof (decimal), typeof (CashDrawerSummaryView), new PropertyMetadata(default(decimal)));

        public decimal CashIn
        {
            get { return (decimal) GetValue(CashInProperty); }
            set { SetValue(CashInProperty, value); }
        }

        #endregion

        #region Cash Out

        public static readonly DependencyProperty CashOutProperty = DependencyProperty.Register(
            "CashOut", typeof (decimal), typeof (CashDrawerSummaryView), new PropertyMetadata(default(decimal)));

        public decimal CashOut
        {
            get { return (decimal) GetValue(CashOutProperty); }
            set { SetValue(CashOutProperty, value); }
        }

        #endregion

        #region Bank Withdraw

        public static readonly DependencyProperty BankWithdrawProperty = DependencyProperty.Register(
            "BankWithdraw", typeof (decimal), typeof (CashDrawerSummaryView), new PropertyMetadata(default(decimal)));

        public decimal BankWithdraw
        {
            get { return (decimal) GetValue(BankWithdrawProperty); }
            set { SetValue(BankWithdrawProperty, value); }
        }

        #endregion

        #region Tips

        public static readonly DependencyProperty TipsProperty = DependencyProperty.Register(
            "Tips", typeof (decimal), typeof (CashDrawerSummaryView), new PropertyMetadata(default(decimal)));

        public decimal Tips
        {
            get { return (decimal) GetValue(TipsProperty); }
            set { SetValue(TipsProperty, value); }
        }

        #endregion

        #region Last Cashier In

        public static readonly DependencyProperty LastCashierInProperty = DependencyProperty.Register(
            "LastCashierIn", typeof (decimal), typeof (CashDrawerSummaryView), new PropertyMetadata(default(decimal)));

        public decimal LastCashierIn
        {
            get { return (decimal) GetValue(LastCashierInProperty); }
            set { SetValue(LastCashierInProperty, value); }
        }

        #endregion

        #region Last Cashier Out

        public static readonly DependencyProperty LastCashierOutProperty = DependencyProperty.Register(
            "LastCashierOut", typeof (decimal), typeof (CashDrawerSummaryView), new PropertyMetadata(default(decimal)));

        public decimal LastCashierOut
        {
            get { return (decimal) GetValue(LastCashierOutProperty); }
            set { SetValue(LastCashierOutProperty, value); }
        }

        #endregion
    }
}
