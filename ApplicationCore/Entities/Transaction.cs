using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Transaction : BaseDateTimeEntity
    {
        [Obsolete("Uses only for EF Core generating")]
        public Transaction()
        { }
        public Transaction(string transactionOwnId, PaymentType paymentType, double paymentAmount)
        {
            TransactionOwnId = transactionOwnId;
            PaymentTypeId = (int)paymentType;
            PaymentAmount = paymentAmount;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string TransactionOwnId { get; set; }
        public int PaymentTypeId { get; set; }
        public double PaymentAmount { get; set; }
    }

    public enum PaymentType
    {
        Unknown = -1,
        GooglePay = 1,
        ApplePay = 2,
        TinkoffAcquiring = 3,
    }
}
