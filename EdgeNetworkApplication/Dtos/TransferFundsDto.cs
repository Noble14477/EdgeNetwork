using System;
using System.Collections.Generic;
using System.Text;

namespace EdgeNetworkApplication.Dtos
{
    public class TransferFundsDto
    {
        public Guid SenderWalletId { get; set; }
        public string ReceiverAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
