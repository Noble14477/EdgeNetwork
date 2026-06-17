using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Enums;

namespace EdgeNetworkApplication.Dtos.Bills
{
    public class PurchaseDataDto
    {
        public Guid WalletId { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public NetworkProvider Provider { get; set; }
        public string PlanId { get; set; } = string.Empty;
        public string Currency { get; set; } = "NGN";
    }
}
