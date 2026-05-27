using System;
using System.Collections.Generic;
using System.Text;

namespace EdgeNetworkDomain.ValueObjects
{
    public sealed class PhoneNumber : IEquatable<PhoneNumber>
    {
        public string Value { get; }

        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Phone number is required.");

            var digits = new string(value.Where(char.IsDigit).ToArray());
            if (digits.Length < 10) throw new ArgumentException("Phone number is too short.");

            Value = value.Trim();
        }

        public bool Equals(PhoneNumber? other) => other is not null && Value == other.Value;
        public override bool Equals(object? obj) => Equals(obj as PhoneNumber);
        public override int GetHashCode() => HashCode.Combine(Value);
        public override string ToString() => Value;
    }
}
