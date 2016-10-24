using System;
using System.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Account;
using YumaPos.Client.App;
using YumaPos.Client.UI.ViewModels.Impl;
using Y_POS.Core.Extensions;
using Y_POS.Core.ViewModels.Items.Contracts;
using Y_POS.Core.ViewModels.Items.Impl;

namespace Y_POS.Core.ViewModels.Pages
{
    public sealed class NavMenuVm : BaseVm, INavMenuVm
    {
        #region Fields

        private readonly IAccountServiceManager _accountServiceManager;

        #endregion

        #region Properties
        
        public string StoreName { get; }
        public string TerminalName { get; }
        public INavMenuItemVm[] Items { get; }
        [Reactive]
        public INavMenuItemVm SelectedItem { get; set; }

        #endregion

        #region Constructor

        public NavMenuVm(IAppService appService, IAccountServiceManager accountServiceManager)
        {
            _accountServiceManager = accountServiceManager;
            if (appService == null) throw new ArgumentNullException(nameof(appService));

            Items = GetNavigationItems();

            this.WhenAnyValue(vm => vm.NavigationService.CurrenUri)
                .SubscribeToObserveOnUi(uri =>
                {
                    SelectedItem = Items.FirstOrDefault(vm => vm.TargetUri.Equals(uri));
                });

            StoreName = appService.Store.Title;
            TerminalName = appService.Terminal.Name;
        }

        #endregion

        #region Private methods

        private INavMenuItemVm[] GetNavigationItems()
        {
            return new INavMenuItemVm[]
            {
                new NavMenuItemVm(AppNavigation.ActiveOrders),
                new NavMenuItemVm(AppNavigation.ClosedOrders),
                new NavMenuItemVm(AppNavigation.Cashdrawer),
                new NavMenuItemVm(AppNavigation.Reports),
                new NavMenuItemVm(AppNavigation.Settings),
                new NavMenuItemVm(_accountServiceManager)
            };
        }

        #endregion
    }
}
