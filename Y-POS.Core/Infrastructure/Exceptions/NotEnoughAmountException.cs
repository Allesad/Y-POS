using System;

namespace Y_POS.Core.Infrastructure.Exceptions
{
    public sealed class NotEnoughAmountException : Exception
    {
        #region Constructors

        public NotEnoughAmountException(decimal requestedAmount, decimal availableamount)
        {
            RequestedAmount = requestedAmount;
            AvailableAmount = availableamount;
        }

        #endregion

        #region Properties

        public decimal RequestedAmount { get; private set; }
        public decimal AvailableAmount { get; private set; }

        #endregion
    }
}
