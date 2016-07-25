using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;

namespace Y_POS.Views.CheckoutParts
{
    /// <summary>
    /// Interaction logic for CheckoutCustomerView.xaml
    /// </summary>
    public partial class CheckoutCustomerView : UserControl
    {
        public CheckoutCustomerView()
        {
            InitializeComponent();

            this.WhenAnyValue(view => view.CustomersList.ItemsSource)
                .Select(enumerable => enumerable != null)
                .Subscribe(b => CustomersList.SetValue(VisibilityProperty, b
                    ? Visibility.Visible
                    : Visibility.Collapsed));

            SearchCustomer.Search += (sender, args) =>
            {
                CustomersList.ItemsSource = SearchCustomer.HasText
                    ? new[]
                    {
                        new Customer("Johnsone, Andrew", "1545 258 6985"),
                        new Customer("Johnson, Alice", "154 445 8788"),
                        new Customer("Johnsone, Daniel", "458 788 2339"),
                        new Customer("Johnsone, Michael", "458 774 8551"),
                        new Customer("Doe, John", "458 788 2339"),
                        new Customer("Morrisson, John", "458 774 8551"),
                        new Customer("Green, Johny", "458 788 2339"),
                        new Customer("Saunders, Johnatan", "458 774 8551")
                    }
                    : null;
            };
        }

        private class Customer
        {
            public string Name { get; private set; }
            public string Phone { get; private set; }

            public Customer(string name, string phone)
            {
                Name = name;
                Phone = phone;
            }
        }
    }
}