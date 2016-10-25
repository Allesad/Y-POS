using System;
using YumaPos.Client.Helpers;
using YumaPos.Client.Navigation;
using YumaPos.Client.UI.ViewModels.Impl;
using Y_POS.Core.Extensions;
using Y_POS.Core.Infrastructure.Exceptions;
using Y_POS.Core.Infrastructure.Notifications;
using Y_POS.Core.Properties;

namespace Y_POS.Core.ViewModels
{
    public abstract class PosPageVm : PageVm
    {
        public IToastManager Toast => ServiceLocator.Resolve<IToastManager>();

        protected override void HandleError(Exception exception)
        {
            if (exception is UnauthorizedException)
            {
                NavigationService.StartIntent(new Intent(AppNavigation.PinLogin).SetFlags(IntentFlags.NoHistory | IntentFlags.NewHistory));
                DialogService.ShowNotificationMessage(Resources.Dialog_Notification_UnauthorizedLogout);
                return;
            }
            if (exception is ServerRuntimeException)
            {
                Logger.Error("Server error", exception);
                DialogService.ShowErrorMessage(Resources.Dialog_Error_ServerRuntimeError);
                return;
            }
            base.HandleError(exception);
        }
    }
}
