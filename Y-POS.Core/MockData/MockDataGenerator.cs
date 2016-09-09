using System;
using YumaPos.Shared.API.Enums;

namespace Y_POS.Core.MockData
{
    internal static class MockDataGenerator
    {
        private static readonly Random Rnd = new Random();

        public static DateTime GetRandomDate(DateTime from, DateTime to)
        {
            var range = to - from;

            var randTimeSpan = new TimeSpan((long)(Rnd.NextDouble() * range.Ticks));

            return from + randTimeSpan;
        }

        public static decimal GetRandomAmount(double minValue, double maxValue)
        {
            var next = Rnd.NextDouble();

            return new decimal(minValue + (next * (maxValue - minValue)));
        }

        public static string GetRandomCustomerName()
        {
            int i = Rnd.Next(0, 10);

            if (i >= 0 && i < 4)
            {
                return "Jack Smartass";
            }
            if (i >= 4 && i < 7)
            {
                return "Arnold Schwarzenegger";
            }
            return "Gabe Newell";
        }

        public static OrderStatus GetOrderStatus()
        {
            int res = Rnd.Next(0, 12);
            res = res == 10 ? ++res : res;
            return (OrderStatus) res;
        }
    }
}
