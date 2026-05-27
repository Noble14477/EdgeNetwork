using System;
using System.Collections.Generic;
using System.Text;
using EdgeNetworkDomain.Interface;
using EdgeNetworkInfrastructure.Data;

namespace EdgeNetworkInfrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;

        public UnitOfWork(AppDBContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }
    }
}
