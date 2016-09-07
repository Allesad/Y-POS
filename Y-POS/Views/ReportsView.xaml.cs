namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for ReportsView.xaml
    /// </summary>
    public partial class ReportsView : BaseView
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
