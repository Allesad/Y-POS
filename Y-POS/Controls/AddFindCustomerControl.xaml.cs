using System.Windows;
using System.Windows.Controls;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Controls
{
    /// <summary>
    /// Interaction logic for AddFindCustomerControl.xaml
    /// </summary>
    public partial class AddFindCustomerControl : UserControl
    {
        public AddFindCustomerControl()
        {
            InitializeComponent();
        }

        #region First name

        public static readonly DependencyProperty FirstNameProperty = DependencyProperty.Register(
            "FirstName", typeof (string), typeof (AddFindCustomerControl), new PropertyMetadata(default(string)));

        public string FirstName
        {
            get { return (string) GetValue(FirstNameProperty); }
            set { SetValue(FirstNameProperty, value); }
        }

        #endregion

        #region Last name

        public static readonly DependencyProperty LastNameProperty = DependencyProperty.Register(
            "LastName", typeof (string), typeof (AddFindCustomerControl), new PropertyMetadata(default(string)));

        public string LastName
        {
            get { return (string) GetValue(LastNameProperty); }
            set { SetValue(LastNameProperty, value); }
        }

        #endregion

        #region Phone

        public static readonly DependencyProperty PhoneProperty = DependencyProperty.Register(
            "Phone", typeof (string), typeof (AddFindCustomerControl), new PropertyMetadata(default(string)));

        public string Phone
        {
            get { return (string) GetValue(PhoneProperty); }
            set { SetValue(PhoneProperty, value); }
        }

        #endregion

        #region Email

        public static readonly DependencyProperty EmailProperty = DependencyProperty.Register(
            "Email", typeof (string), typeof (AddFindCustomerControl), new PropertyMetadata(default(string)));

        public string Email
        {
            get { return (string) GetValue(EmailProperty); }
            set { SetValue(EmailProperty, value); }
        }

        #endregion

        #region Search text

        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register(
            "SearchText", typeof (string), typeof (AddFindCustomerControl), new PropertyMetadata(default(string), OnSearchStringChanged));

        public string SearchText
        {
            get { return (string) GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        private static void OnSearchStringChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var control = (AddFindCustomerControl)dependencyObject;

            if (control.CustomersList == null) return;

            control.CustomersList.Visibility = !string.IsNullOrEmpty(control.SearchText)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        #endregion

        #region Customers

        public static readonly DependencyProperty CustomersProperty = DependencyProperty.Register(
            "Customers", typeof (ICustomerItemVm[]), typeof (AddFindCustomerControl), new PropertyMetadata(default(ICustomerItemVm)));

        public ICustomerItemVm[] Customers
        {
            get { return (ICustomerItemVm[]) GetValue(CustomersProperty); }
            set { SetValue(CustomersProperty, value); }
        }

        #endregion

        #region Selected customer

        public static readonly DependencyProperty SelectedCustomerProperty = DependencyProperty.Register(
            "SelectedCustomer", typeof (ICustomerItemVm), typeof (AddFindCustomerControl), new PropertyMetadata(default(ICustomerItemVm)));

        public ICustomerItemVm SelectedCustomer
        {
            get { return (ICustomerItemVm) GetValue(SelectedCustomerProperty); }
            set { SetValue(SelectedCustomerProperty, value); }
        }

        #endregion
    }
}
