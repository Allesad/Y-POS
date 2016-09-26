using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DialogManagement.Contracts;
using YumaPos.Client.Helpers;
using YumaPos.Client.Navigation.Contracts;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : BaseView, IDialogHost
    {
        #region Fields

        private readonly Stack<DialogWindow> _dialogs = new Stack<DialogWindow>(3);

        #endregion

        public MainView()
        {
            InitializeComponent();

            var dlgManager = ServiceLocator.Resolve<IDialogManager>();
            dlgManager.SetHost(this);
        }

        public void ShowDialog(IBaseDialog dialog)
        {
            // Hack to avoid blocking current method until dialog window is closed.
            // Problem is that when we call dialog as modal window through ShowDialog() it blocks the current method execution
            // until the dialog is closed. But if it is closed it means that TaskCompletionSource for current dialog (in case when we call it 
            // via ShowAsync()) is set to null but we're still need to extract Task from it to return to original caller.
            // All this shit leads to NullReferenceException while ShowAsync() method call. Dialog infrastructure neeeds to be revisited.
            Dispatcher.InvokeAsync(() =>
            {
                var dlgWindow = new DialogWindow(dialog)
                {
                    Owner = _dialogs.Count > 0 ? _dialogs.Peek() : Application.Current.MainWindow
                };

                _dialogs.Push(dlgWindow);

                ShadowOverlay.Visibility = Visibility.Visible;
                dlgWindow.ShowDialog();
            });
        }

        public void HideDialog()
        {
            var dlg = _dialogs.Pop();
            dlg.Close();
            if (_dialogs.Count == 0)
            {
                ShadowOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void OnBrowseBack(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                ServiceLocator.Resolve<INavigator>().Back();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnSwitchMenuState(object sender, ExecutedRoutedEventArgs e)
        {
            NavMenuContainer.Visibility = NavMenuContainer.Visibility == Visibility.Collapsed
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void Page_OnContentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            NavMenuContainer.Visibility = Visibility.Collapsed;
        }
    }
}
