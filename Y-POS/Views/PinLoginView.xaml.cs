using System;
using System.Windows;
using System.Windows.Controls;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for PinLoginView.xaml
    /// </summary>
    public partial class PinLoginView : UserControl
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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ((Window)Window.GetWindow(this)).Content = new LoginView();
        }

        private void ButtonBase_OnClick1(object sender, RoutedEventArgs e)
        {
            ((Window)Window.GetWindow(this)).Content = new ActiveOrdersView();
        }
    }
}
