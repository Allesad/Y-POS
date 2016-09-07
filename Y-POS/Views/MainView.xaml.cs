using System;
using System.Windows;
using System.Windows.Input;
using DialogManagement.Contracts;
using DialogManagement.Core;
using YumaPos.Client.Helpers;
using YumaPos.Client.Navigation.Contracts;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : BaseView, IDialogHost
    {
        public MainView()
        {
            InitializeComponent();

            var dlgManager = ServiceLocator.Resolve<IDialogManager>();
            dlgManager.SetHost(this);
        }

        public void ShowDialog(IBaseDialog dialog)
        {
            MessageBox.Show(((TextDialogContent) dialog.Content).Message);
        }

        public void HideDialog()
        {
            
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
