using System.Windows.Controls;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for ReportsView.xaml
    /// </summary>
    public partial class ReportsView : UserControl
    {
        public ReportsView()
        {
            InitializeComponent();

            ReportsList.ItemsSource = new[]
            {
                "SALES REPORT",
                "SHIFT REPORT"
            };

            ReportsList.SelectedIndex = 0;
        }
    }
}
