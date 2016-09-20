using System;
using System.Threading.Tasks;
using DialogManagement.Contracts;
using Y_POS.Core.Properties;

namespace Y_POS.Core.Extensions
{
    public static class DialogEx
    {
        public static void ShowErrorMessage(this IDialogManager dialogManager, string message)
        {
            if (dialogManager == null) throw new ArgumentNullException(nameof(dialogManager));
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Argument is null or empty", nameof(message));

            dialogManager.CreateMessageDialog(message, Resources.Dialog_Title_Error).Show();
        }

        public static void ShowNotificationMessage(this IDialogManager dialogManager, string message)
        {
            if (dialogManager == null) throw new ArgumentNullException(nameof(dialogManager));
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Argument is null or empty", nameof(message));

            dialogManager.CreateMessageDialog(message, Resources.Dialog_Title_Notification).Show();
        }

        private static Task<bool> ShowConfirmationAsync(this IDialogManager dialogManager, string message)
        {
            if (dialogManager == null) throw new ArgumentNullException(nameof(dialogManager));
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Argument is null or empty", nameof(message));

            return dialogManager.CreateConfirmationDialog(message, Resources.Dialog_Title_Confirmation).ShowAsync();
        } 
    }
}
