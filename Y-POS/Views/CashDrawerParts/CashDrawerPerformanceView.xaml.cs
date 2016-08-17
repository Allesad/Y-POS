using System;
using System.Windows.Controls;

namespace Y_POS.Views.CashDrawerParts
{
    /// <summary>
    /// Interaction logic for CashDrawerPerformanceView.xaml
    /// </summary>
    public partial class CashDrawerPerformanceView : UserControl
    {
        private static readonly Random Rnd = new Random();

        public CashDrawerPerformanceView()
        {
            InitializeComponent();

            PerformanceList.ItemsSource = new[]
            {
                new { Time = DateTime.Now, Activity = "Cashier In", Amount = RandomAmount(), StaffName = "Melissa Jefferson" },
                new { Time = DateTime.Now, Activity = "Cashier In", Amount = RandomAmount(), StaffName = "Melissa Jefferson" },
                new { Time = DateTime.Now, Activity = "Cashier In", Amount = RandomAmount(), StaffName = "Melissa Jefferson" },
                new { Time = DateTime.Now, Activity = "Transaction #325", Amount = RandomAmount(), StaffName = "Melissa Jefferson" },
                new { Time = DateTime.Now, Activity = "Cashier In", Amount = RandomAmount(), StaffName = "Melissa Jefferson" },
                new { Time = DateTime.Now, Activity = "Cashier In", Amount = RandomAmount(), StaffName = "Melissa Jefferson" },
                new { Time = DateTime.Now, Activity = "Bank Withdraw", Amount = RandomAmount(), StaffName = "Melissa Jefferson" },
                new { Time = DateTime.Now, Activity = "Cashier Out", Amount = RandomAmount(), StaffName = "Melissa Jefferson" },
                new { Time = DateTime.Now, Activity = "Cashier In", Amount = RandomAmount(), StaffName = "Melissa Jefferson" },
                new { Time = DateTime.Now, Activity = "Cashier In", Amount = RandomAmount(), StaffName = "Melissa Jefferson" },
                new { Time = DateTime.Now, Activity = "Cashier In", Amount = RandomAmount(), StaffName = "Melissa Jefferson" },
                new { Time = DateTime.Now, Activity = "Cashier In", Amount = RandomAmount(), StaffName = "Melissa Jefferson" },
                new { Time = DateTime.Now, Activity = "Cashier In", Amount = RandomAmount(), StaffName = "Melissa Jefferson" },
                new { Time = DateTime.Now, Activity = "Cashier In", Amount = RandomAmount(), StaffName = "Melissa Jefferson" },
            };
        }

        public static decimal RandomAmount()
        {
            return NextDecimal(10, 500);
        }

        public static decimal NextDecimal(decimal from, decimal to)
        {
            byte fromScale = new System.Data.SqlTypes.SqlDecimal(from).Scale;
            byte toScale = new System.Data.SqlTypes.SqlDecimal(to).Scale;

            byte scale = (byte)(fromScale + toScale);
            if (scale > 28)
                scale = 28;

            decimal r = new decimal(Rnd.Next(), Rnd.Next(), Rnd.Next(), false, scale);
            if (Math.Sign(from) == Math.Sign(to) || from == 0 || to == 0)
                return decimal.Remainder(r, to - from) + from;

            bool getFromNegativeRange = (double)from + Rnd.NextDouble() * ((double)to - (double)from) < 0;
            return getFromNegativeRange ? decimal.Remainder(r, -from) + from : decimal.Remainder(r, to);
        }
    }
}
