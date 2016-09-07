using System.Globalization;
using System.Reactive.Concurrency;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using ReactiveUI;
using YumaPos.Client.Helpers;
using YumaPos.Common.Infrastructure.IoC;
using Y_POS.Bootstrap;
using Y_POS.Core.ViewModels;
using Y_POS.Views;

namespace Y_POS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.Name = "Y-POS.Win";
            const string lang = "ru-RU";
            //const string lang = "en-US";
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);

            CultureInfo.DefaultThreadCurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            CultureInfo.DefaultThreadCurrentCulture = Thread.CurrentThread.CurrentCulture;

            // ReSharper disable once AccessToStaticMemberViaDerivedType
            Inline.LanguageProperty.OverrideMetadata(
                typeof(Inline),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            
            base.OnStartup(e);

            RxApp.MainThreadScheduler = DispatcherScheduler.Current;
            RxApp.TaskpoolScheduler = TaskPoolScheduler.Default;

            Bootstrap();
        }

        private void Bootstrap()
        {
            var resolver = Bootstrapper.Run();

            ServiceLocator.Init(resolver);

            ShowUi(resolver);
        }

        private void ShowUi(IResolver resolver)
        {
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;

            IAppMainVm mainVm = resolver.Resolve<IAppMainVm>();
            mainVm.Init();
            MainWindow = new MainWindow
            {
                Content = new MainView
                {
                    DataContext = mainVm
                }
            };
            MainWindow.Left = screenWidth / 2f - MainWindow.Width / 2f;
            MainWindow.Top = screenHeight / 2f - MainWindow.Height / 2f;
            MainWindow.Show();
        }
    }
}
