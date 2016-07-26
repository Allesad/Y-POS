﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Y_POS.Views.CheckoutParts;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for CheckoutView.xaml
    /// </summary>
    public partial class CheckoutView : UserControl
    {
        public CheckoutView()
        {
            InitializeComponent();

            Content.Content = new CheckoutPaymentView();
            
            OperationsList.SetValue(ItemsControl.ItemsSourceProperty, new[]
            {
                new OperationItem((Geometry) FindResource("CustomerIcon"), Core.Properties.Resources.Customer, "Add Customer"),
                new OperationItem((Geometry) FindResource("PercentIcon"), Core.Properties.Resources.Discount, "10% - Happy Hour"),
                new OperationItem((Geometry) FindResource("DivideIcon"), Core.Properties.Resources.Split, "All on One"),
                new OperationItem((Geometry) FindResource("PromoIcon"), Core.Properties.Resources.Promo, "No")
            });
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            ((Window)Window.GetWindow(this)).Content = new OrderMakerView();
        }

        private void SwitchToPayment(object sender, RoutedEventArgs e)
        {
            Content.Content = new CheckoutPaymentView();
        }

        private void OperationsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserControl content = null;
            switch (OperationsList.SelectedIndex)
            {
                case 0:
                    content = new CheckoutCustomerView();
                    break;
                case 1:
                    content = new CheckoutDiscountView();
                    break;
                case 3:
                    content = new CheckoutPromoView();
                    break;
            }
            if (content != null)
            {
                Content.Content = content;
            }
        }

        private class OperationItem
        {
            public Geometry Icon { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }

            public OperationItem(Geometry icon, string title, string content)
            {
                Icon = icon;
                Title = title;
                Content = content;
            }
        }
    }
}
