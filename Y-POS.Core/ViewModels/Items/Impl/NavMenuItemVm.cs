using System;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using YumaPos.Client.Account;
using YumaPos.Client.Navigation;
using YumaPos.Client.UI.ViewModels.Impl;
using Y_POS.Core.Extensions;
using Y_POS.Core.Properties;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Core.ViewModels.Items.Impl
{
    public class NavMenuItemVm : PosBaseVm, INavMenuItemVm
    {
        #region Properties

        public NavUri TargetUri { get; }
        public string Title { get; }

        #endregion

        #region Commands

        public ICommand CommandNavigate { get; }

        #endregion

        #region Constructor

        public NavMenuItemVm(NavUri targetUri)
        {
            TargetUri = targetUri;

            Title = GetTitle(targetUri);

            var cmd = ReactiveCommand.Create();
            cmd.Where(_ => !TargetUri.Equals(NavigationService.CurrenUri))
                .SubscribeToObserveOnUi(
                    _ => NavigationService.StartIntent(new Intent(TargetUri).SetFlags(IntentFlags.ClearTop)));
            CommandNavigate = cmd;
        }

        public NavMenuItemVm(IAccountServiceManager accountServiceManager)
        {
            if (accountServiceManager == null) throw new ArgumentNullException(nameof(accountServiceManager));

            var accManager = accountServiceManager;

            TargetUri = AppNavigation.PinLogin;

            Title = GetTitle(TargetUri);

            var cmd = ReactiveCommand.CreateAsyncTask(_ => accManager.Logout());
            cmd.ThrownExceptions.Subscribe(HandleError);
            cmd.SubscribeToObserveOnUi(_ => NavigationService.StartIntent(new Intent(TargetUri).SetFlags(IntentFlags.NoHistory)));
            CommandNavigate = cmd;
        }

        #endregion

        private static string GetTitle(NavUri targetUri)
        {
            if (targetUri.Equals(AppNavigation.ActiveOrders))
            {
                return Resources.Navigation_ActiveOrders;
            }
            if (targetUri.Equals(AppNavigation.ClosedOrders))
            {
                return Resources.Navigation_ClosedOrders;
            }
            if (targetUri.Equals(AppNavigation.Cashdrawer))
            {
                return Resources.Navigation_Cashdrawer;
            }
            if (targetUri.Equals(AppNavigation.Reports))
            {
                return Resources.Navigation_Reports;
            }
            if (targetUri.Equals(AppNavigation.Settings))
            {
                return Resources.Navigation_Settings;
            }
            if (targetUri.Equals(AppNavigation.PinLogin))
            {
                return Resources.Navigation_Quit;
            }
            return null;
        }
    }
}
