using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Common;
using EdgeNetworkDomain.Enums;
using EdgeNetworkDomain.Enums.Bills;
using EdgeNetworkDomain.ValueObjects;

namespace EdgeNetworkDomain.Entities
{
    public class BillPayment : BaseEntity
    {
        public Guid walletId { get; set; }
        public Wallet Wallet { get; set; } = null;
        public BillType BillType { get; set; }
        public NetworkProvider Provider { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string? DataPlanId { get; set; }
        public Money Amount { get; set; } = null;
        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

        private BillPayment() { }

        public static BillPayment Create(
            Guid walletId,
            BillType billType,
            NetworkProvider provider,
            string phonNumber,
            Money amount,
            string? dataPlanId = null
        )
        {
            return new BillPayment
            {
                Id = Guid.NewGuid(),
                walletId = walletId,
                BillType = billType,
                Provider = provider,
                PhoneNumber = phonNumber,
                Amount = amount,
                DataPlanId = dataPlanId,
                Status = TransactionStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
        }
        public void MarkCompleted() => Status = TransactionStatus.Completed;
        public void MarkFailed() => Status = TransactionStatus.Failed;
    }

    
}
