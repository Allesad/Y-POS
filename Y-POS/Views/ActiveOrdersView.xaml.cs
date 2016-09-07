using System;
using System.Windows;

namespace Y_POS.Views
{
    /// <summary>
    /// Interaction logic for ActiveOrdersView.xaml
    /// </summary>
    public partial class ActiveOrdersView : BaseView
    {
        static readonly Random rnd = new Random();

        public ActiveOrdersView()
        {
            InitializeComponent();

            int lastOrderNumber = 125;
            int ordersCount = 10;
            var arr = new Order[ordersCount];
            for (int i = 0; i < ordersCount; i++)
            {
                arr[i] = new Order(lastOrderNumber--, GetRandomDate(System.DateTime.Today, System.DateTime.UtcNow), "New", "Unknown Customer", (decimal)GetRandomAmount(10, 50));
            }

            OrdersList.ItemsSource = arr;
        }

        private static DateTime GetRandomDate(DateTime from, DateTime to)
        {
            var range = to - from;

            var randTimeSpan = new TimeSpan((long)(rnd.NextDouble() * range.Ticks));

            return from + randTimeSpan;
        }
        
        private static double GetRandomAmount(double minValue, double maxValue)
        {
            var next = rnd.NextDouble();

            return minValue + (next * (maxValue - minValue));
        }

        private class Order
        {
            public int OrderNumber { get; set; }
            public DateTime DateCreated { get; set; }
            public string Status { get; set; }
            public string CustomerName { get; set; }
            public decimal Amount { get; set; }

            public Order()
            {
            }

            public Order(int orderNumber, DateTime dateCreated, string status, string customerName, decimal amount)
            {
                OrderNumber = orderNumber;
                DateCreated = dateCreated;
                Status = status;
                CustomerName = customerName;
                Amount = amount;
            }
        }
    }
}