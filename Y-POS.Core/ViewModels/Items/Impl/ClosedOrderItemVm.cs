using System;
using System.Collections.Generic;
using System.Linq;
using YumaPos.Shared.API.Enums;
using YumaPos.Shared.API.Models;
using YumaPos.Shared.Core.Utils.Formating;

namespace Y_POS.Core.ViewModels.Items.Impl
{
    public sealed class ClosedOrderItemVm : PosBaseVm
    {
        #region Properties

        public Guid OrderId { get; set; }
        public int OrderNumber { get; }
        public DateTime DateCreated { get; }
        public DateTime DateCreatedUtc { get; }
        public decimal Amount { get; }
        public string TransactionInfo { get; }
        public OrderType OrderType { get; }
        public string PaymentType { get; }
        public CustomerDto Customer { get; }
        public EmployeeDto Employee { get; }
        public bool HasMultipleTransactions { get; }

        public IEnumerable<OrderTransactionItemVm> Transactions { get; }

        public string CustomerName { get; }
        public string EmployeeName { get; }

        #endregion

        #region Constructor

        public ClosedOrderItemVm(RestaurantOrderDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            OrderId = dto.OrderId;
            OrderNumber = dto.Number;
            DateCreated = dto.Created.ToLocalTime();
            DateCreatedUtc = dto.Created;
            Amount = dto.Amount;
            OrderType = dto.Type;
            PaymentType = GetPaymentType(dto);
            Customer = dto.CustomerDto;
            Employee = dto.EmployeeDto;
            Transactions = dto.Transactions != null
                ? dto.Transactions.Select(t => new OrderTransactionItemVm(t)).ToArray()
                : new OrderTransactionItemVm[0];
            HasMultipleTransactions = dto.Transactions.Count() > 1;

            CustomerName = Customer != null ? FormattingUtils.FullName(Customer.FirstName, Customer.LastName) : "-";
            EmployeeName = Employee != null ? FormattingUtils.FullName(Employee.FirstName, Employee.LastName) : "-";

            if (!HasMultipleTransactions)
            {
                var t = dto.Transactions.FirstOrDefault();
                if (t == null)
                {
                    TransactionInfo = dto.Status.ToString();
                    return;
                }
                
                TransactionInfo = t.TransactionNumber.ToString();
            }
            else
            {
                TransactionInfo = $"{dto.Transactions.Count()} transactions";
            }
        }

        #endregion

        private static string GetPaymentType(RestaurantOrderDto dto)
        {
            if (dto.Transactions == null || !dto.Transactions.Any()) return string.Empty;

            var tenders = dto.Transactions.Select(t => t.Tenders);
            string paymenType;

            if (tenders.All(tender => tender.PaymentCode.Equals("CA", StringComparison.OrdinalIgnoreCase)))
            {
                paymenType = "Cash";
            }
            else if (tenders.All(tender => tender.PaymentCode.Equals("CC", StringComparison.OrdinalIgnoreCase)))
            {
                paymenType = "Card";
            }
            else if (tenders.All(tender => tender.PaymentCode.Equals("EG", StringComparison.OrdinalIgnoreCase)))
            {
                paymenType = "Gift Card";
            }
            else
            {
                paymenType = "Multi";
            }

            return paymenType;
        }
    }

    public sealed class OrderTransactionItemVm : PosBaseVm
    {
        #region Properties

        public DateTime Date { get; }
        public decimal Amount { get; }
        public int Number { get; }
        public string Type { get; }
        public string PaidBy { get; }
        public CustomerDto Customer { get; }
        public EmployeeDto Employee { get; }
        public string CustomerName { get; }
        public string EmployeeName { get; }
        
        #endregion

        #region Constructor

        public OrderTransactionItemVm(TransactionDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            Date = dto.TransactionDate;
            Amount = dto.Tenders.TransactionAmount;
            Number = dto.TransactionNumber;
            Type = dto.TransactionType;
            PaidBy = dto.Tenders.PaymentTypeName;
            Customer = dto.Customer;
            Employee = dto.Employee;
            CustomerName = Customer != null ? FormattingUtils.FullName(Customer.FirstName, Customer.LastName) : "-";
            EmployeeName = Employee != null ? FormattingUtils.FullName(Employee.FirstName, Employee.LastName) : "-";
        }

        #endregion
    }
}
