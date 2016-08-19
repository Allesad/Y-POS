using System.Windows.Controls;
using System.Windows.Media;
using Y_POS.Views.SettingsParts;

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
                new { Icon = (Geometry) FindResource("PrintIcon"), Title = "HARDWARE" }
            };

            SettingsList.SelectedIndex = 1;

            DetailsContainer.Content = new SettingsHardwareView();
        }
    }
}
