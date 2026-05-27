using System;
using System.Collections.Generic;
using System.Text;

namespace EdgeNetworkDomain.ValueObjects
{
    public sealed class Money : IEquatable<Money>
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency)
        {
            if (amount < 0) throw new ArgumentException("Amount cannot be negative.");
            if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency cannot be null or empty.");

            Amount = amount;
            Currency = currency.ToUpperInvariant();
        }

        public Money Add(Money other)
        {
            if (Currency != other.Currency) throw new InvalidOperationException("Cannot add different currencies.");
            return new Money(Amount + other.Amount, Currency);
        }
        public Money Subract(Money other)
        {
            if (Currency != other.Currency) throw new InvalidOperationException("Currencey mismatch.");
            if (Amount < other.Amount) throw new InvalidOperationException("Insufficient funds");
            return new Money(Amount - other.Amount, Currency);
        }

        public bool Equals(Money? other) => other is not null && Amount == other.Amount && Currency == other.Currency;
        public override bool Equals(object? obj) => Equals(obj as Money);
        public override int GetHashCode() => HashCode.Combine(Amount, Currency);
        public static bool operator ==(Money a, Money b) => a.Equals(b);
        public static bool operator !=(Money a, Money b) => !a.Equals(b);
        public override string ToString() => $"{Currency} {Amount:N2}";

    }
}
