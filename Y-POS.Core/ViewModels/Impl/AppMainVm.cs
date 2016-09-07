using System;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using YumaPos.Client.Common;
using YumaPos.Client.Navigation;
using YumaPos.Client.UI.ViewModels.Contracts;
using YumaPos.Client.UI.ViewModels.Impl;

namespace Y_POS.Core.ViewModels
{
    public sealed class AppMainVm : LifecycleVm, IAppMainVm
    {
        #region Fields

        #endregion

        #region Properties

        public INavMenuVm NavMenuVm { get; }

        [Reactive]
        public IPageVm CurrentPage { get; private set; }

        #endregion

        public AppMainVm(INavMenuVm navMenuVm)
        {
            if (navMenuVm == null) throw new ArgumentNullException(nameof(navMenuVm));

            NavMenuVm = navMenuVm;
        }

        protected override void InitLifetimeSubscriptions()
        {
            AddLifetimeSubscription(this.WhenAnyValue(vm => vm.NavigationService.CurrentPage)
                .Select(page => page as IPageVm)
                .Where(page => page != null)
                .ObserveOn(SchedulerService.UiScheduler)
                .Subscribe(vm =>
                {
                    CurrentPage = vm;
                }));
        }

        protected override void OnCreate(IArgsBundle args)
        {
        }

        protected override void OnStart()
        {
            NavigationService.StartIntent(new Intent(AppNavigation.Login));
        }
    }
}
