using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Entities;

namespace EdgeNetworkDomain.Interface
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetByWalletIdAsync(Guid walletId, CancellationToken ct = default);
    }
}
