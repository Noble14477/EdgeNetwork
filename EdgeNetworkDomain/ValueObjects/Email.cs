using System;
using System.Collections.Generic;
using System.Text;

namespace EdgeNetworkDomain.ValueObjects
{
    public sealed class Email : IEquatable<Email>
    {
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Email is required.");
            if (!value.Contains('@')) throw new ArgumentException("Email is not valid.");

            Value = value.Trim().ToLowerInvariant();
        }

        public bool Equals(Email? other) => other is not null && Value == other.Value;
        public override bool Equals(object? obj) => Equals(obj as Email);
        public override int GetHashCode() => HashCode.Combine(Value);
        public override string ToString() => Value;
    }
}
