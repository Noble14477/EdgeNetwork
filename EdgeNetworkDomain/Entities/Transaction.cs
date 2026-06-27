using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Common;
using EdgeNetworkDomain.Enums;
using EdgeNetworkDomain.ValueObjects;

namespace EdgeNetworkDomain.Entities
{
    public class Transaction : BaseEntity
    {
        public Guid WalletId { get; private set; }
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }
        public TransactionType Type { get; private set; }
        public TransactionStatus Status { get; private set; } = TransactionStatus.Pending;
        public string Reference { get; private set; }
        public string? Description { get; private set; }

        private Transaction() { }

        public static Transaction Create(Guid walletId, Money amount, TransactionType type, string? description = null, string? reference = null)
        {
            return new Transaction
            {
                WalletId = walletId,
                Amount = amount.Amount,
                Currency= amount.Currency,
                Type = type,
                Reference = Guid.NewGuid().ToString("N").ToUpper(),
                Description = description
            };
        }

        public void MarkCompleted() => Status = TransactionStatus.Completed;
        public void MarkFailed() => Status = TransactionStatus.Failed;
    }
}
