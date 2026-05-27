using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Entities;

namespace EdgeNetworkDomain.Interface
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        Task<Wallet?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct = default);
        Task<IEnumerable<Wallet>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    }
}
