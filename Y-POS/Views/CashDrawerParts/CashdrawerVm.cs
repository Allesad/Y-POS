namespace Y_POS.Views.CashDrawerParts
{
    internal class CashdrawerVm
    {
        #region Properties

        public object[] Items { get; private set; }

        #endregion

        public CashdrawerVm()
        {
            Items = new[]
            {
                new {Value = 1, Qty = 5, IsCoins = false, Amount = 0m},
                new {Value = 5, Qty = 2, IsCoins = false, Amount = 0m},
                new {Value = 10, Qty = 4, IsCoins = false, Amount = 0m},
                new {Value = 20, Qty = 6, IsCoins = false, Amount = 0m},
                new {Value = 50, Qty = 1, IsCoins = false, Amount = 0m},
                new {Value = 100, Qty = 2, IsCoins = false, Amount = 0m},
                new {Value = 0, Qty = 0, IsCoins = true, Amount = 3.50m}
            };
        }
    }
}