using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;

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
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

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
    }
}
