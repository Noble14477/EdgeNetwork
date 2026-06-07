using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Enums;

namespace EdgeNetworkApplication.Dtos
{
    public class TransactionFilterDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public TransactionType? Type { get; set; }
        public TransactionStatus? Status { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
