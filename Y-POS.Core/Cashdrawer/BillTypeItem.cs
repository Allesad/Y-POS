namespace Y_POS.Core.Cashdrawer
{
    public sealed class BillTypeItem
    {
        public bool IsCoins { get; private set; }
        public int Multiplier { get; private set; }

        public BillTypeItem() : this(true, 0)
        {
        }

        public BillTypeItem(int multiplier) : this(false, multiplier)
        {
        }

        private BillTypeItem(bool isCoins, int multiplier)
        {
            IsCoins = isCoins;
            Multiplier = multiplier;
        }
    }
}
