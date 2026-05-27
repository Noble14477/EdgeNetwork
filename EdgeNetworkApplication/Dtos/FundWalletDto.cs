using System;
using System.Collections.Generic;
using System.Text;

namespace EdgeNetworkApplication.Dtos
{
    public class FundWalletDto
    {
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
