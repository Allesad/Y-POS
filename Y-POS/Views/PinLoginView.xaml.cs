using System.Windows;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for PinLoginView.xaml
    /// </summary>
    public partial class PinLoginView : BaseView
    {
        public PinLoginView()
        {
            InitializeComponent();

            Keypad.ButtonClick += KeypadOnButtonClick;
        }

        private void KeypadOnButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked: " + e.OriginalSource);
        }
    }
}
