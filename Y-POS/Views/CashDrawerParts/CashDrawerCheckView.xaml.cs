using System;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Pages;

namespace Y_POS.Views.CashDrawerParts
{
    /// <summary>
    /// Interaction logic for CashDrawerInitView.xaml
    /// </summary>
    public partial class CashDrawerCheckView : UserControl
    {
        #region Fields

        private ReactiveList<BillTypeItemViewVm> _billTypes;

        #endregion

        #region Properties

        public decimal Amount { get; private set; }

        public CashdrawerVm ViewModel => (CashdrawerVm)DataContext;

        #endregion

        public CashDrawerCheckView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        #region Private methods

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ResetFields();
            AmountInput.Focus();

            this.WhenAnyValue(view => view.ViewModel.State)
                .Where(
                    state =>
                        state == CashdrawerState.CashierIn || state == CashdrawerState.CashierOut ||
                        state == CashdrawerState.Check)
                .Select(GetTitleForState)
                .SubscribeToObserveOnUi(title => TitleTbl.Text = title);

            this.WhenAnyValue(view => view.ViewModel.CashierBy)
                .SubscribeToObserveOnUi(cashierBy =>
                {
                    if (cashierBy == CashierBy.Amount)
                    {
                        AmountInput.Focus();
                    }
                });

            if (_billTypes == null)
            {
                _billTypes = new ReactiveList<BillTypeItemViewVm> { ChangeTrackingEnabled = true };

                this.WhenAnyValue(view => view.ViewModel.BillTypes)
                    .Where(items => items != null)
                    .Take(1)
                    .Select(
                        types =>
                            types.Select(
                                item => item.IsCoins ? new BillTypeItemViewVm() : new BillTypeItemViewVm(item.Multiplier)))
                    .SubscribeToObserveOnUi(vms =>
                    {
                        _billTypes.Clear();
                        _billTypes.AddRange(vms);
                    });

                this.WhenAnyObservable(view => view._billTypes.ItemChanged)
                    .Where(_ => ViewModel.CashierBy == CashierBy.BillType)
                    .Select(_ => _billTypes.Sum(vm => vm.Total))
                    .SubscribeToObserveOnUi(total => AmountInput.Text = total.ToString("F"));

                BillsList.ItemsSource = _billTypes;
            }
        }

        private static string GetTitleForState(CashdrawerState state)
        {
            switch (state)
            {
                case CashdrawerState.CashierIn:
                    return Core.Properties.Resources.Cashdrawer_CashierIn;
                case CashdrawerState.CashierOut:
                    return Core.Properties.Resources.Cashdrawer_CashierOut;
                case CashdrawerState.Check:
                    return Core.Properties.Resources.Cashdrawer_Check;
            }
            return string.Empty;
        }

        private void AmountInput_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            decimal value;
            Amount = decimal.TryParse(tb.Text, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat, out value) ? value : 0;
        }

        private void ResetFields()
        {
            AmountInput.Text = "0";
            foreach (var billType in _billTypes)
            {
                billType.Qty = 0;
                billType.Value = 0;
            }
        }

        #endregion
    }

    internal class BillTypeItemViewVm : ReactiveObject
    {
        #region Properties

        public bool IsCoins { get; }
        [Reactive]
        public int Qty { get; set; }
        [Reactive]
        public decimal Value { get; set; }
        public int Multiplier { get; }

        public decimal Total { [ObservableAsProperty] get; }

        #endregion

        #region Constructor

        public BillTypeItemViewVm() : this(true, 0)
        {
        }

        public BillTypeItemViewVm(int multiplier) : this(false, multiplier)
        {
            if (multiplier < 1)
                throw new ArgumentOutOfRangeException(nameof(multiplier), "Currency value cannot be lower than 1!");
        }

        private BillTypeItemViewVm(bool isCoins, int multiplier)
        {
            IsCoins = isCoins;
            Multiplier = multiplier;
            Qty = 0;
            Value = 0;

            this.WhenAny(vm => vm.IsCoins, vm => vm.Value, vm => vm.Multiplier, vm => vm.Qty,
                (isCoinz, valuez, multiplierz, qtyz) => isCoinz.Value
                    ? valuez.Value
                    : qtyz.Value * (decimal)multiplierz.Value)
                .ToPropertyEx(this, vm => vm.Total);
        }

        #endregion
    }
}