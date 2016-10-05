using System;
using System.Collections.Generic;
using System.Linq;
using YumaPos.Client.Services;
using YumaPos.Shared.API.Models;
using Y_POS.Core.Infrastructure;

namespace Y_POS.Core.Checkout
{
    public class ReceiptItem : IEquatable<ReceiptItem>
    {
        #region Properties

        public int SplittingNumber { get; }
        public decimal Total { get; }
        public decimal Subtotal { get; }
        public decimal Change { get; }
        public decimal TotalPaid { get; }
        public bool IsPaid { get; }
        public bool IsRefunded { get; }
        public bool IsVoid { get; }
        public bool IsTaxExempt { get; }
        public bool IsMultipleTendersPayment => Tenders.Count() > 1;
        public IEnumerable<TenderParams> Tenders { get; }
        
        #endregion

        #region Constructors

        public ReceiptItem(RestaurantOrderReceiptDto model) : this(model, false)
        {
        }

        public ReceiptItem(RestaurantOrderReceiptDto model, bool isVoid)
        {
            Guard.NotNull(model, nameof(model));

            SplittingNumber = model.SplittingNumber;
            Total = model.TotalAmount + model.Tips;
            Subtotal = model.SubTotal;
            TotalPaid = model.TotalPaid;
            Change = model.Change;
            IsPaid = model.TransactionId != null;
            IsRefunded = model.IsRefunded;
            IsVoid = isVoid;
            IsTaxExempt = model.IsTaxExempt;

            Tenders =
                model.Tenders.ToArray().Select(tender => new TenderParams {Amount = tender.TenderAmount, TenderType = tender.Type});


        }

        #endregion

        #region Overridden methods

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((ReceiptItem) obj);
        }

        public bool Equals(ReceiptItem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return SplittingNumber == other.SplittingNumber 
                && Total == other.Total 
                && Subtotal == other.Subtotal 
                && Change == other.Change 
                && TotalPaid == other.TotalPaid 
                && IsPaid == other.IsPaid 
                && IsRefunded == other.IsRefunded 
                && IsVoid == other.IsVoid 
                && IsTaxExempt == other.IsTaxExempt 
                && Equals(Tenders, other.Tenders);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = SplittingNumber;
                hashCode = (hashCode * 397) ^ Total.GetHashCode();
                hashCode = (hashCode * 397) ^ Subtotal.GetHashCode();
                hashCode = (hashCode * 397) ^ Change.GetHashCode();
                hashCode = (hashCode * 397) ^ TotalPaid.GetHashCode();
                hashCode = (hashCode * 397) ^ IsPaid.GetHashCode();
                hashCode = (hashCode * 397) ^ IsRefunded.GetHashCode();
                hashCode = (hashCode * 397) ^ IsVoid.GetHashCode();
                hashCode = (hashCode * 397) ^ IsTaxExempt.GetHashCode();
                hashCode = (hashCode * 397) ^ (Tenders?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        #endregion
    }
}
