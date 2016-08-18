using System.Windows.Controls;
using System.Windows.Media;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();

            SettingsList.ItemsSource = new[]
            {
                new { Icon = (Geometry) FindResource("SettingsIcon"), Title = "GENERAL SETTINGS" },
                new { Icon = (Geometry) FindResource("CurrencyIcon"), Title = "PAYMENT PROCESS" },
                new { Icon = (Geometry) FindResource("PrintIcon"), Title = "PRINTERS" }
            };

            SettingsList.SelectedIndex = 0;
        }
    }
}
