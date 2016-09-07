using System.Windows.Controls;

namespace Y_POS.Views.CashDrawerParts
{
    /// <summary>
    /// Interaction logic for CashDrawerAmountUpdateView.xaml
    /// </summary>
    public partial class CashDrawerAmountUpdateView : UserControl
    {
        private string _title;

        public CashDrawerAmountUpdateView()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                UpdateTitle.Text = _title;
            }
        }
    }
}
