using System;
using System.Collections.Generic;
using System.Text;

namespace EdgeNetworkDomain.ValueObjects
{
    public sealed class FullName : IEquatable<FullName>
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string FullNameString =>$"{FirstName} {LastName}";

        public FullName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name cannot be null or empty.");
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
        }

        public bool Equals(FullName? other) => other is not null && FirstName == other.FirstName && LastName == other.LastName;
        public override bool Equals(object? obj) => Equals(obj as FullName);
        public override int GetHashCode() => HashCode.Combine(FirstName, LastName);
        public override string ToString() => FullNameString;

    }
}
