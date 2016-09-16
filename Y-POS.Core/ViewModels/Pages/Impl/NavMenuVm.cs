using System;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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

        private readonly IAppService _appService;

        #endregion

        #region Properties
        
        public string StoreName { get; }
        public string TerminalName { get; }
        public INavMenuItemVm[] Items { get; }
        [Reactive]
        public INavMenuItemVm SelectedItem { get; set; }

        #endregion

        #region Constructor

        public NavMenuVm(IAppService appService)
        {
            if (appService == null) throw new ArgumentNullException(nameof(appService));

            _appService = appService;

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

        private static INavMenuItemVm[] GetNavigationItems()
        {
            return new[]
            {
                new NavMenuItemVm(AppNavigation.ActiveOrders),
                new NavMenuItemVm(AppNavigation.ClosedOrders),
                new NavMenuItemVm(AppNavigation.Cashdrawer),
                new NavMenuItemVm(AppNavigation.Reports),
                new NavMenuItemVm(AppNavigation.Settings),
                new NavMenuItemVm(AppNavigation.PinLogin)
            };
        }

        #endregion
    }
}
