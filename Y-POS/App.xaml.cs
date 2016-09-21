using System;
using System.Globalization;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Threading;
using NLog;
using ReactiveUI;
using YumaPos.Client.App;
using YumaPos.Client.Helpers;
using YumaPos.Common.Infrastructure.IoC;
using YumaPos.Common.Infrastructure.Logging;
using YumaPos.WPF.UI.Controls;
using Y_POS.Bootstrap;
using Y_POS.Core.ViewModels.Pages;
using Y_POS.Views;

namespace Y_POS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
            // Disable range notification to avoid "Range actions are not supported" in ListCollectionView from call from ReactiveList
            RxApp.SupportsRangeNotifications = false;

            InitExceptionHandlers();
            Bootstrap();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomainOnUnhandledException;
            Current.DispatcherUnhandledException -= DispatcherOnUnhandledException;
            TaskScheduler.UnobservedTaskException -= TaskSchedulerOnUnobservedTaskException;

            base.OnExit(e);
        }

        private void InitExceptionHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Current.DispatcherUnhandledException += DispatcherOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
        }

        private async void Bootstrap()
        {
            var resolver = Bootstrapper.Run();

            LoggerHelper.LoggingService = resolver.Resolve<ILoggingService>();
            ServiceLocator.Init(resolver);
            await resolver.Resolve<IAppServiceManager>().InitAsync();

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

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            var ex = e.Exception;
            Current.Dispatcher.Invoke(() => LastStandExceptionHandler(ex));
        }

        private static void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            LastStandExceptionHandler(e.Exception);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                LastStandExceptionHandler(ex);
            }
        }

        private static void LastStandExceptionHandler(Exception ex)
        {
            Logger.Fatal(ex, "Unhandled exception!");

            MessageBox.Show(Current.MainWindow, ex.Message, "Fatal exception");

            Current.Shutdown();
        }
    }
}
