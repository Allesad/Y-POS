using System;
using System.Globalization;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ReactiveUI;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Pages;
using Y_POS.Views.CashDrawerParts;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for CashDrawerView.xaml
    /// </summary>
    public partial class CashDrawerView : BaseView
    {
        #region Fields

        private CashDrawerCheckView _checkView;
        private CashDrawerAmountUpdateView _amountUpdateView;
        private CashDrawerPerformanceView _performanceView;

        #endregion

        #region Properties
        
        private CashdrawerVm ViewModel => (CashdrawerVm)DataContext;

        #endregion

        #region Constructor

        public CashDrawerView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        #endregion

        #region Overridden methods

        protected override void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            base.OnLoaded(sender, routedEventArgs);

            var stateStream = this.WhenAnyValue(view => view.ViewModel.State).Publish().RefCount();

            stateStream
                .Select(GetViewForState)
                .SubscribeToObserveOnUi(uc => ContentContainer.Content = uc);

            stateStream.SubscribeToObserveOnUi(UpdateActionBar);

            stateStream.Where(state => state != CashdrawerState.PerformanceLog)
                .Select(GetMainButtonTitleForState)
                .SubscribeToObserveOnUi(title => MainActionButton.Title = title);

            stateStream.Where(state => state != CashdrawerState.PerformanceLog)
                .Select(GetCommandForState)
                .SubscribeToObserveOnUi(command => MainActionButton.Command = command);

            UpdateActionBar(ViewModel.State);
            if (ViewModel.State != CashdrawerState.PerformanceLog)
            {
                MainActionButton.Command = GetCommandForState(ViewModel.State);
            }
        }

        #endregion

        #region Private methods

        private UserControl GetViewForState(CashdrawerState state)
        {
            switch (state)
            {
                case CashdrawerState.CashierIn:
                case CashdrawerState.CashierOut:
                case CashdrawerState.Check:
                    _checkView = _checkView ?? new CashDrawerCheckView { DataContext = DataContext };
                    return _checkView;
                case CashdrawerState.CashIn:
                case CashdrawerState.CashOut:
                case CashdrawerState.BankWithdraw:
                case CashdrawerState.AddTips:
                    _amountUpdateView = _amountUpdateView ?? new CashDrawerAmountUpdateView { DataContext = DataContext };
                    return _amountUpdateView;
                case CashdrawerState.PerformanceLog:
                    _performanceView = _performanceView ?? new CashDrawerPerformanceView { DataContext = DataContext };
                    return _performanceView;
            }
            return null;
        }

        private static string GetMainButtonTitleForState(CashdrawerState state)
        {
            switch (state)
            {
                case CashdrawerState.CashierIn:
                    return Core.Properties.Resources.Cashdrawer_CashierIn.ToUpper(CultureInfo.CurrentUICulture);
                case CashdrawerState.CashierOut:
                    return Core.Properties.Resources.Cashdrawer_CashierOut.ToUpper(CultureInfo.CurrentUICulture);
                case CashdrawerState.Check:
                    return Core.Properties.Resources.Cashdrawer_Check.ToUpper(CultureInfo.CurrentUICulture);
                case CashdrawerState.BankWithdraw:
                    return Core.Properties.Resources.Cashdrawer_BankWithdraw.ToUpper(CultureInfo.CurrentUICulture);
                case CashdrawerState.AddTips:
                    return Core.Properties.Resources.Cashdrawer_AddTips.ToUpper(CultureInfo.CurrentUICulture);
                case CashdrawerState.CashIn:
                    return Core.Properties.Resources.Cashdrawer_CashIn.ToUpper(CultureInfo.CurrentUICulture);
                case CashdrawerState.CashOut:
                    return Core.Properties.Resources.Cashdrawer_CashOut.ToUpper(CultureInfo.CurrentUICulture);
                default:
                    throw new ArgumentException("Invalid state! " + state);
            }
        }

        private ICommand GetCommandForState(CashdrawerState state)
        {
            switch (state)
            {
                case CashdrawerState.CashierIn:
                    return ViewModel.CommandCashierIn;
                case CashdrawerState.CashierOut:
                    return ViewModel.CommandCashierOut;
                case CashdrawerState.PerformanceLog:
                    throw new ArgumentException("Invalid state! " + state);
                default:
                    return ViewModel.CommandPerformOperation;
            }
        }

        private void UpdateActionBar(CashdrawerState state)
        {
            switch (state)
            {
                case CashdrawerState.PerformanceLog:
                    MainActionButton.Visibility = Visibility.Collapsed;
                    SendActionButton.Visibility =
                        PrintActionButton.Visibility = FilterActionButton.Visibility = Visibility.Visible;
                    break;
                default:
                    MainActionButton.Visibility = Visibility.Visible;
                    SendActionButton.Visibility =
                        PrintActionButton.Visibility = FilterActionButton.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void MainActionButton_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Released)
            {
                SetActionButtonParameters();
            }
        }

        private void SetActionButtonParameters()
        {
            var state = ViewModel.State;
            switch (state)
            {
                case CashdrawerState.CashierIn:
                case CashdrawerState.CashierOut:
                    MainActionButton.CommandParameter = _checkView.Amount;
                    break;
                case CashdrawerState.Check:
                    MainActionButton.CommandParameter = new object[]
                    {
                        _checkView.Amount,
                        string.Empty
                    };
                    break;
                default:
                    if (state != CashdrawerState.PerformanceLog)
                    {
                        var parameter = new object[]
                        {
                            _amountUpdateView.Amount,
                            _amountUpdateView.Reason
                        };
                        MainActionButton.CommandParameter = parameter;
                    }
                    break;
            }
        }

        #endregion
    }
}
