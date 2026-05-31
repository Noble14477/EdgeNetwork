using System;
using System.Collections.Generic;
using System.Text;

namespace EdgeNetworkApplication.Dtos
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
