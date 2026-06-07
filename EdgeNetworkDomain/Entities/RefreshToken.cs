using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Common;

namespace EdgeNetworkDomain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? RevokedAt { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
