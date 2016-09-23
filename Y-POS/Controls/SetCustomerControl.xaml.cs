using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;
using Y_POS.Core.ViewModels.Items.Contracts;

namespace Y_POS.Controls
{
    /// <summary>
    /// Interaction logic for SetCustomerControl.xaml
    /// </summary>
    public partial class SetCustomerControl : UserControl
    {
        public SetCustomerControl()
        {
            InitializeComponent();

            /*this.WhenAnyValue(control => control.SelectedCustomer)
                .Select(customer => customer != null)
                .Subscribe(
                    hasSelection =>
                    {
                        CustomerDetailsContainer.Visibility = DetailsHeaderContainer.Visibility = hasSelection
                            ? Visibility.Visible
                            : Visibility.Collapsed;
                        SearchBox.Visibility = CustomersList.Visibility = hasSelection
                            ? Visibility.Collapsed
                            : Visibility.Visible;
                    });*/
        }

        #region IsNarrow

        public static readonly DependencyProperty IsNarrowProperty = DependencyProperty.Register(
            "IsNarrow", typeof (bool), typeof (SetCustomerControl), new PropertyMetadata(false, OnIsNarrowChanged));

        private static void OnIsNarrowChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var control = (SetCustomerControl) dependencyObject;

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

        /*#region First name

        public static readonly DependencyProperty FirstNameProperty = DependencyProperty.Register(
            "FirstName", typeof(string), typeof(SetCustomerControl), new PropertyMetadata(default(string)));

        public string FirstName
        {
            get
            {
                return (string)GetValue(FirstNameProperty);
            }
            set
            {
                SetValue(FirstNameProperty, value);
            }
        }

        #endregion

        #region Last name

        public static readonly DependencyProperty LastNameProperty = DependencyProperty.Register(
            "LastName", typeof(string), typeof(SetCustomerControl), new PropertyMetadata(default(string)));

        public string LastName
        {
            get
            {
                return (string)GetValue(LastNameProperty);
            }
            set
            {
                SetValue(LastNameProperty, value);
            }
        }

        #endregion

        #region Phone

        public static readonly DependencyProperty PhoneProperty = DependencyProperty.Register(
            "Phone", typeof(string), typeof(SetCustomerControl), new PropertyMetadata(default(string)));

        public string Phone
        {
            get
            {
                return (string)GetValue(PhoneProperty);
            }
            set
            {
                SetValue(PhoneProperty, value);
            }
        }

        #endregion

        #region Email

        public static readonly DependencyProperty EmailProperty = DependencyProperty.Register(
            "Email", typeof(string), typeof(SetCustomerControl), new PropertyMetadata(default(string)));

        public string Email
        {
            get
            {
                return (string)GetValue(EmailProperty);
            }
            set
            {
                SetValue(EmailProperty, value);
            }
        }

        #endregion

        #region Search text

        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register(
            "SearchText", typeof(string), typeof(SetCustomerControl), new PropertyMetadata(default(string)));

        public string SearchText
        {
            get
            {
                return (string)GetValue(SearchTextProperty);
            }
            set
            {
                SetValue(SearchTextProperty, value);
            }
        }

        private static void OnSearchStringChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var control = (SetCustomerControl)dependencyObject;

            if (control.CustomersList == null)
                return;

            /*control.CustomersList.Visibility = !string.IsNullOrEmpty(control.SearchText)
                ? Visibility.Visible
                : Visibility.Collapsed;#1#
        }

        #endregion

        #region Customers

        public static readonly DependencyProperty CustomersProperty = DependencyProperty.Register(
            "Customers", typeof(ICustomerItemVm[]), typeof(SetCustomerControl), new PropertyMetadata(default(ICustomerItemVm)));

        public ICustomerItemVm[] Customers
        {
            get
            {
                return (ICustomerItemVm[])GetValue(CustomersProperty);
            }
            set
            {
                SetValue(CustomersProperty, value);
            }
        }

        #endregion

        #region Selected customer

        public static readonly DependencyProperty SelectedCustomerProperty = DependencyProperty.Register(
            "SelectedCustomer", typeof(ICustomerItemVm), typeof(SetCustomerControl), new PropertyMetadata(default(ICustomerItemVm), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var control = (SetCustomerControl) dependencyObject;

            control.IsDetailsVisible = args.NewValue != null;
        }

        public ICustomerItemVm SelectedCustomer
        {
            get
            {
                return (ICustomerItemVm)GetValue(SelectedCustomerProperty);
            }
            set
            {
                SetValue(SelectedCustomerProperty, value);
            }
        }

        #endregion

        #region IsDetailsVisible

        public static readonly DependencyProperty IsDetailsVisibleProperty = DependencyProperty.Register(
            "IsDetailsVisible", typeof (bool), typeof (SetCustomerControl), new PropertyMetadata(default(bool)));

        public bool IsDetailsVisible
        {
            get { return (bool) GetValue(IsDetailsVisibleProperty); }
            set { SetValue(IsDetailsVisibleProperty, value); }
        }

        #endregion*/

        private void GoToFindCustomer(object sender, RoutedEventArgs e)
        {
            AddCustomerContainer.Visibility = Visibility.Collapsed;
            FindCustomerContainer.Visibility = Visibility.Visible;
        }

        private void GoToAddCustomer(object sender, RoutedEventArgs e)
        {
            AddCustomerContainer.Visibility = Visibility.Visible;
            FindCustomerContainer.Visibility = Visibility.Collapsed;
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            CustomersList.SelectedItem = null;
        }
    }
}
