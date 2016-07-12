using ReactiveUI;
using Splat;
using YumaPos.Client.UI.ViewModels.Impl;

namespace Y_POS.Core.ViewModels
{
    public sealed class AppMainVm : LifecycleVm, IAppMainVm
    {
        public RoutingState Router { get; private set; }

        public AppMainVm(RoutingState router = null)
        {
            Router = router ?? new RoutingState();

            Router.NavigateAndReset.Execute(Locator.Current.GetService<ILoginVm>());
        }
    }
}
