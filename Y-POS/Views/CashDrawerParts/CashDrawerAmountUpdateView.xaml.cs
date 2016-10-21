using System;
using System.Globalization;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Pages;

namespace Y_POS.Views.CashDrawerParts
{
    /// <summary>
    /// Interaction logic for CashDrawerAmountUpdateView.xaml
    /// </summary>
    public partial class CashDrawerAmountUpdateView : UserControl
    {
        #region Fields

        #endregion

        #region Properties

        private CashdrawerVm ViewModel => (CashdrawerVm) DataContext;
        public decimal Amount { get; private set; }
        public string Reason { get; private set; }

        #endregion

        #region Constructor

        public CashDrawerAmountUpdateView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        #endregion

        #region Private methods

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ResetFields();
            AmountTb.Focus();

            this.WhenAnyValue(view => view.ViewModel.State)
                .Where(
                    state =>
                        state == CashdrawerState.BankWithdraw || state == CashdrawerState.CashIn ||
                        state == CashdrawerState.CashOut)
                .Select(GetTitleForState)
                .SubscribeToObserveOnUi(title => UpdateTitle.Text = title);
        }

        private static string GetTitleForState(CashdrawerState state)
        {
            switch (state)
            {
                case CashdrawerState.BankWithdraw:
                    return Core.Properties.Resources.Cashdrawer_BankWithdraw.ToUpper(CultureInfo.CurrentUICulture);
                case CashdrawerState.CashIn:
                    return Core.Properties.Resources.Cashdrawer_CashIn.ToUpper(CultureInfo.CurrentUICulture);
                case CashdrawerState.CashOut:
                    return Core.Properties.Resources.Cashdrawer_CashOut.ToUpper(CultureInfo.CurrentUICulture);
                default:
                    throw new ArgumentException("Invalid state! " + state);
            }
        }

        private void AmountInput_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;

            decimal value;
            Amount = decimal.TryParse(tb.Text, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat, out value) ? value : 0;
        }

        private void ResetFields()
        {
            AmountTb.Text = "0";
            AmountTb.SelectAll();
            ReasonTb.Text = string.Empty;
        }

        #endregion
    }
}
