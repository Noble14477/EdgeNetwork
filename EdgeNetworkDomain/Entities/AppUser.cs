using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Common;
using EdgeNetworkDomain.Enums;
using EdgeNetworkDomain.ValueObjects;

namespace EdgeNetworkDomain.Entities
{
    public class AppUser : BaseEntity
    {
        public FullName FullName { get; private set; }
        public Email Email { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public UserStatus Status { get; private set; } = UserStatus.Active;
        public ICollection<Wallet> Wallets { get; private set; } = new List<Wallet>();

        private AppUser() { } 

        public static AppUser Create(Guid id, string firstName, string lastName, string email, string phoneNumber)
        {
            return new AppUser
            {
                Id= id,
                FullName = new FullName(firstName, lastName),
                Email = new Email(email),
                PhoneNumber = new PhoneNumber(phoneNumber)
            };
        }

    }
}
