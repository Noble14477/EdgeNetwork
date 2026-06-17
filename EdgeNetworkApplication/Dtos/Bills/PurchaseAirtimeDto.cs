using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Enums;

namespace EdgeNetworkApplication.Dtos.Bills
{
    public class PurchaseAirtimeDto
    {
        public Guid WalletId { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public NetworkProvider Provider { get; set; }
        public decimal Amount { get; set; }
        public string Currency {  get; set; } = "NGN";
    }
}
