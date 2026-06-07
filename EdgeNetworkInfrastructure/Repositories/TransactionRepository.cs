using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Common;
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

        public async Task<(IEnumerable<Transaction> Items, int TotalCount)> GetFilteredAsync(Guid walletId, TransactionFilter filter, CancellationToken ct = default)
        {
            var query = _context.Transactions.Where(t=> t.WalletId == walletId).AsQueryable();

            if (filter.Type.HasValue)
            {
                query = query.Where(t => t.Type == filter.Type.Value);
            }

            if(filter.Status.HasValue)
            {
                query = query.Where(t => t.Status == filter.Status.Value);
            }

            if (filter.DateFrom.HasValue)
            {
                query = query.Where(t => t.CreatedAt >= filter.DateFrom.Value);
            }

            if (filter.DateTo.HasValue)
            {
                query = query.Where(t => t.CreatedAt <= filter.DateTo.Value);
            }

            var totalCount = await query.CountAsync(ct);

            var items = await query.OrderByDescending(t => t.CreatedAt)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }
    }
}
