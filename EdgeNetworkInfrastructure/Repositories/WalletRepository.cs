using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Entities;
using EdgeNetworkDomain.Interface;
using EdgeNetworkInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EdgeNetworkInfrastructure.Repositories
{
    public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
    {
        public WalletRepository(AppDBContext context) : base(context) { }

        public async Task<Wallet?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct = default)
        {
            return await _context.Wallets
                .FirstOrDefaultAsync(w => w.AccountNumber.Value == accountNumber, ct);
        }

        public async Task<IEnumerable<Wallet>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _context.Wallets
                .Where(w => w.UserId == userId)
                .ToListAsync(ct);
        }
    }
}
