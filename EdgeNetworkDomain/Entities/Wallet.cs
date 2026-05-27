using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Common;
using EdgeNetworkDomain.Enums;
using EdgeNetworkDomain.ValueObjects;

namespace EdgeNetworkDomain.Entities
{
    public class Wallet : BaseEntity
    {
        public Guid UserId { get; private set; }
        public AccountNumber AccountNumber { get; private set; }
        public Money Balance { get; private set; }
        public WalletStatus Status { get; private set; } = WalletStatus.Active;
        public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

        private Wallet() { } // For EF Core

        public static Wallet Create(Guid userId, string currency)
        {
            return new Wallet
            {
                UserId = userId,
                AccountNumber = AccountNumber.Generate(),
                Balance = new Money(0, currency)
            };
        }

        public void Credit(Money amount)
        {
            if (Status != WalletStatus.Active)
                throw new InvalidOperationException("Cannot credit a non-active wallet.");
            Balance = Balance.Add(amount);
        }

        public void Debit(Money amount)
        {
            if (Status != WalletStatus.Active)
                throw new InvalidOperationException("Cannot debit a non-active wallet.");
            Balance = Balance.Subract(amount);
        }

    }
}
