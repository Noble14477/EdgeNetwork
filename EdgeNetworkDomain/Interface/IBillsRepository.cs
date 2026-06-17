using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Entities;

namespace EdgeNetworkDomain.Interface
{
    public interface IBillsRepository
    {
        Task AddAsync(BillPayment billPayment, CancellationToken ct = default);
        void Update(BillPayment billPayment);
    }
}
