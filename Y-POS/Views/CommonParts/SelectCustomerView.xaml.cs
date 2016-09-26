using System.Windows;

namespace Y_POS.Views.CommonParts
{
    /// <summary>
    /// Interaction logic for SetCustomerControl.xaml
    /// </summary>
    public partial class SelectCustomerView : BaseView
    {
        public SelectCustomerView()
        {
            InitializeComponent();
        }

        #region IsNarrow

        public static readonly DependencyProperty IsNarrowProperty = DependencyProperty.Register(
            "IsNarrow", typeof (bool), typeof (SelectCustomerView), new PropertyMetadata(false, OnIsNarrowChanged));

        private static void OnIsNarrowChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var control = (SelectCustomerView) dependencyObject;

            control.CustomerPhoto.Width = control.IsNarrow ? 150 : 200;
            control.CustomerPhoto.Height = control.IsNarrow ? 150 : 200;
            control.FormContainer.Height = control.IsNarrow ? 150 : 200;
        }

        public bool IsNarrow
        {
            get { return (bool) GetValue(IsNarrowProperty); }
            set { SetValue(IsNarrowProperty, value); }
        }

        #endregion

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            CustomersList.SelectedItem = null;
        }
    }
}
