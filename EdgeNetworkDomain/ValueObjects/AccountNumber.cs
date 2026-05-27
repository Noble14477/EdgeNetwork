using System;
using System.Collections.Generic;
using System.Text;

namespace EdgeNetworkDomain.ValueObjects
{
    public sealed class AccountNumber : IEquatable<AccountNumber>
    {
        public string Value { get; }

        public AccountNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Account number is required.");
            Value = value;
        }

        // Called by Wallet.Create() to auto-generate a new account number
        public static AccountNumber Generate()
        {
            var number = new Random().NextInt64(1000000000, 9999999999).ToString();
            return new AccountNumber(number);
        }

        public bool Equals(AccountNumber? other) => other is not null && Value == other.Value;
        public override bool Equals(object? obj) => Equals(obj as AccountNumber);
        public override int GetHashCode() => HashCode.Combine(Value);
        public override string ToString() => Value;
    }
}
