using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Entities;
using EdgeNetworkDomain.Interface;
using EdgeNetworkInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EdgeNetworkInfrastructure.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDBContext context) : base(context) { }

        public async Task<IEnumerable<Transaction>> GetByWalletIdAsync(Guid walletId, CancellationToken ct = default)
        {
            return await _context.Transactions
                .Where(t => t.WalletId == walletId)
                .ToListAsync(ct);
        }
    }
}
