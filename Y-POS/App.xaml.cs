using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using ReactiveUI;
using Splat;

namespace Y_POS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.Name = "Imidus.Win";
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
        }

        private void Bootstrap()
        {
            var resolver = Locator.CurrentMutable;

            resolver.RegisterLazySingleton(() => this, typeof(IScreen));
        }
    }
}
