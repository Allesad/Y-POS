using System.Windows.Controls;

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