using System.Windows.Controls;

namespace Y_POS.Views.SettingsParts
{
    /// <summary>
    /// Interaction logic for SettingsHardwareView.xaml
    /// </summary>
    public partial class SettingsHardwareView : UserControl
    {
        public SettingsHardwareView()
        {
            InitializeComponent();

            var printers = new[]
            {
                "Printer 1",
                "Printer 2",
                "Printer 3"
            };

            var cashdrawers = new[]
            {
                "Cashdrawer 1",
                "Cashdrawer 2"
            };

            var msrs = new[]
            {
                "Msr 1",
                "Msr 2",
                "Msr 3"
            };

            var scanners = new[]
            {
                "Scanner 1",
                "Scanner 2"
            };

            ReceiptPrinters.ItemsSource = KitchenPrinters.ItemsSource = printers;
            Cashdrawers.ItemsSource = cashdrawers;
            Msrs.ItemsSource = msrs;
            Scanners.ItemsSource = scanners;
        }
    }
}
